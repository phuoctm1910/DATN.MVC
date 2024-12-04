new Vue({
    el: '#call-app',
    data: {
        callState: {
            isCallActive: false,
            isConnecting: false,
            isRejected: false
        },
        localStream: null,
        remoteStream: null,
        peerConnection: null,
        isCaller: false,
        isInvitationAccepted: false
    },
    created() {
        this.initializeSignalR();
    },
    methods: {
        async initializeSignalR() {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/chathub")
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            try {
                await this.connection.start();
                console.log("SignalR connected for call");
            } catch (err) {
                console.error("SignalR connection error:", err);
            }

            this.connection.onreconnected(() => {
                console.log("SignalR reconnected");
            });

            // Handle incoming call request
            this.connection.on("ReceiveCallRequest", this.handleReceiveCallRequest);

            // Handle response to call invitation
            this.connection.on("CallInvitationResponse", this.handleInvitationResponse);

            // WebRTC signaling events
            this.connection.on("ReceiveOffer", this.handleReceiveOffer);
            this.connection.on("ReceiveAnswer", this.handleReceiveAnswer);
            this.connection.on("ReceiveIceCandidate", this.handleReceiveIceCandidate);
        },

        async startCall() {
            try {
                const roomId = await this.connection.invoke("CreateRoom", this.callerId, this.friendId, this.callerName);

                console.log("Room created:", roomId);

                // Open call window for the caller to wait for response
                const callWindow = window.open(`/Call/Call?roomId=${roomId}&callerId=${this.callerId}`, '_blank');
                if (!callWindow) {
                    console.error("Failed to open call window for caller.");
                    alert("Please allow popups to start the call.");
                }
            } catch (err) {
                console.error("Error starting call:", err);
            }
        },

        async handleReceiveCallRequest(roomId, callerId, callerName) {
            console.log(`Incoming call from ${callerName} in room ${roomId}`);

            // Open a call window with accept/reject options
            const callWindow = window.open(`/Call/Call?roomId=${roomId}&callerId=${callerId}`, '_blank');
            if (!callWindow) {
                console.error("Failed to open call window for receiver.");
                alert("Please allow popups to receive the call.");
            }
        },

        async handleInvitationResponse(response, roomId) {
            if (response === "accepted") {
                console.log(`Invitation accepted for room ${roomId}`);
                this.isInvitationAccepted = true;

                // Automatically join the room for the caller
                if (this.isCaller) {
                    this.joinRoom(roomId);
                    this.callState.isCallActive = true;
                    this.initializeWebRTC();
                }
            } else if (response === "rejected") {
                console.log(`Invitation rejected for room ${roomId}`);
                alert("The call was rejected.");
            }
        },

        async joinCallRoom(roomId) {
            try {
                await this.connection.invoke("JoinRoom", roomId, this.friendId);
                console.log("Joined room:", roomId);
            } catch (err) {
                console.error("Error joining room:", err);
            }
        },

        acceptCall() {
            console.log("Call accepted.");
            this.connection.invoke("RespondToInvitation", this.chatRoomId, "accepted")
                .then(() => {
                    this.callState.isCallActive = true;
                    this.joinRoom(this.chatRoomId);
                    this.initializeWebRTC();
                })
                .catch(err => console.error("Error accepting call invitation:", err));
        },

        rejectCall() {
            console.log("Call rejected.");
            this.callState.isRejected = true;

            this.connection.invoke("RespondToInvitation", this.chatRoomId, "rejected")
                .then(() => console.log("Call rejection sent to caller"))
                .catch(err => console.error("Error rejecting call invitation:", err));
        },

        initializeWebRTC() {
            this.peerConnection = new RTCPeerConnection({
                iceServers: [{ urls: "stun:stun.l.google.com:19302" }]
            });

            this.peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    this.connection.invoke("SendIceCandidate", this.chatRoomId, event.candidate)
                        .catch(err => console.error("Error sending ICE Candidate:", err));
                }
            };

            this.peerConnection.ontrack = (event) => {
                this.remoteStream = event.streams[0];
                document.getElementById('remote-video').srcObject = this.remoteStream;
            };

            navigator.mediaDevices.getUserMedia({ video: true, audio: true })
                .then((stream) => {
                    this.localStream = stream;
                    document.getElementById('local-video').srcObject = stream;

                    stream.getTracks().forEach((track) => {
                        this.peerConnection.addTrack(track, stream);
                    });
                })
                .catch(err => {
                    console.error("Error accessing media devices:", err);
                    alert("Failed to access your camera or microphone.");
                });
        },

        handleReceiveOffer(offer) {
            const rtcOffer = new RTCSessionDescription(offer);

            this.peerConnection.setRemoteDescription(rtcOffer)
                .then(() => this.peerConnection.createAnswer())
                .then((answer) => this.peerConnection.setLocalDescription(answer))
                .then(() => {
                    this.connection.invoke("SendAnswer", this.chatRoomId, this.peerConnection.localDescription);
                })
                .catch(err => console.error("Error handling offer:", err));
        },

        handleReceiveAnswer(answer) {
            const rtcAnswer = new RTCSessionDescription(answer);

            this.peerConnection.setRemoteDescription(rtcAnswer)
                .catch(err => console.error("Error handling answer:", err));
        },

        handleReceiveIceCandidate(candidate) {
            const iceCandidate = new RTCIceCandidate(candidate);

            this.peerConnection.addIceCandidate(iceCandidate)
                .catch(err => console.error("Error adding ICE candidate:", err));
        },

        endCall() {
            this.callState.isCallActive = false;

            if (this.peerConnection) {
                this.peerConnection.close();
                this.peerConnection = null;
            }

            if (this.localStream) {
                this.localStream.getTracks().forEach((track) => track.stop());
                this.localStream = null;
            }

            this.connection.invoke("EndCall", this.chatRoomId)
                .then(() => console.log("Call ended"))
                .catch(err => console.error("Error ending call:", err));
        }
    }
});
