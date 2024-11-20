new Vue({
    el: '#sidebar-app',
    data: {
        friends: [],
        selectedFriend: null,
        userId: userLogged,
        newPostFor: 0,
        isMinimized: false,
        connection: null,
        isImageModalVisible: false,
        selectedImageUrl: '',
        isImageSliderVisible: false,
        selectedImageIndex: 0,
        allImages: [],
        swiperInstance: null // Swiper instance
    },
    created() {
        this.loadFriendOfUser();
        this.initializeSignalR();
    },
    methods: {
        formatTime(timestamp) {
            const messageDate = new Date(timestamp * 1000); // Unix timestamp (s) to milliseconds
            const now = new Date();
            const timeDifference = now - messageDate;
            const oneDay = 24 * 60 * 60 * 1000; // One day in milliseconds
            const oneWeek = 7 * oneDay;

            if (timeDifference < oneDay && messageDate.getDate() === now.getDate()) {
                // Trong ngày
                return messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
            } else if (timeDifference < oneWeek) {
                // Trong tuần
                const dayNames = ["Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy"];
                const dayOfWeek = dayNames[messageDate.getDay()];
                return `${dayOfWeek} ${messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
            } else {
                // Ngoài tuần
                return messageDate.toLocaleString("vi-VN", {
                    weekday: 'long',
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });
            }
        },
        async loadFriendOfUser() {
            const userData = {
                UserId: this.userId,
                Status: 1
            };
            try {
                const response = await fetch(`/Friend/GetListFriendOfUser`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    },
                    body: JSON.stringify(userData)
                });
                const data = await response.json();
                this.friends = data.apiData;
            } catch (error) {
                console.error('Error fetching friends:', error);
            }
        },
        initializeSignalR() {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/chathub")
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            this.connection.serverTimeoutInMilliseconds = 60000;

            this.connection.start().then(() => {
                console.log("SignalR connected");
            }).catch(err => console.error("SignalR connection error:", err));

            this.connection.on("UpdateMessageStatus", (messageId, status) => {
                const message = this.$refs.chatbox.messages.find(msg => msg.id === messageId);
                if (message) {
                    message.status = status; // Cập nhật trạng thái tin nhắn
                }
            });



            this.connection.on("ReceiveMessage", (senderId, content, timestamp, messageId, images) => {
                if (this.selectedFriend && this.selectedFriend.friendUserId === senderId) {
                    const newMessage = {
                        id: messageId,
                        text: content,
                        isSent: false,
                        timestamp: timestamp,
                        images: images || [] // Gán danh sách URL ảnh nếu có
                    };
                    this.$refs.chatbox.messages.push(newMessage);

                    this.markMessageAsRead();

                    this.playNotificationSound();
                }
            });


            this.connection.on("MessageRead", (messageId) => {
                const message = this.$refs.chatbox.messages.find((msg) => msg.id === messageId);
                if (message) {
                    message.status = 2; // Đã đọc
                }
            });


            this.connection.on("MessagesMarkedAsRead", (messageIds) => {
                messageIds.forEach(id => {
                    const message = this.$refs.chatbox.messages.find(msg => msg.id === id);
                    if (message) {
                        message.status = 2; // Đã đọc
                    }
                });
            });


            this.connection.onclose(error => {
                console.error("Kết nối SignalR bị đóng:", error);
                this.errorMessage = "Mất kết nối tới máy chủ. Đang thử kết nối lại...";
            });

            this.connection.onreconnected(connectionId => {
                console.log("Kết nối SignalR đã được khôi phục:", connectionId);
                this.errorMessage = "";
            }); 

        },
        playNotificationSound() {
            const audio = new Audio('/sound/notification.mp3');  // Make sure the path is correct
            audio.play().catch(error => {
                console.error("Error playing notification sound:", error);
            });
        },
        async markMessageAsRead() {
            if (this.selectedFriend) {
                const unreadMessages = this.$refs.chatbox.messages.filter(
                    (msg) => msg.status !== 2 && !msg.isSent
                );
                if (unreadMessages.length > 0) {
                    const messageIds = unreadMessages.map((msg) => msg.id);
                    try {
                        // Gửi danh sách ID tin nhắn tới SignalR Hub để đánh dấu là "Đã đọc"
                        await this.connection.invoke(
                            "MarkMessagesAsRead",
                            this.selectedFriend.chatRoomId,
                            messageIds
                        );

                        // Cập nhật trạng thái tin nhắn trong giao diện
                        unreadMessages.forEach((msg) => {
                            msg.status = 2; // Đã đọc
                        });
                    } catch (error) {
                        console.error("Lỗi khi đánh dấu tin nhắn là đã đọc:", error);
                    }
                }
            }
        },
        async openChatBox(friend) {
            this.selectedFriend = friend;
            this.isMinimized = false;

            if (!friend.chatRoomId) {
                try {
                    const chatRoomData = {
                        User1Id: this.userId,
                        User2Id: this.selectedFriend.friendUserId
                    };
                    const response = await fetch(`/Chat/CreateChatRoom`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8'
                        },
                        body: JSON.stringify(chatRoomData)
                    });
                    const result = await response.json();
                    if (result.success) {
                        this.selectedFriend.chatRoomId = result.chatRoomId;
                        this.connection.invoke("JoinChatRoom", this.selectedFriend.chatRoomId);
                    } else {
                        console.error(result.message);
                        return;
                    }
                } catch (error) {
                    console.error("Error creating chat room:", error);
                    return;
                }
            } else {
                await this.connection.invoke("JoinChatRoom", friend.chatRoomId);
            }

            try {
                const messageResponse = await fetch(`/Chat/GetMessages?chat_roomId=${this.selectedFriend.chatRoomId}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    }
                });
                const messageData = await messageResponse.json();

                this.$refs.chatbox.messages = messageData.apiData.map(msg => ({
                    id: msg.id,
                    text: msg.content,
                    isSent: msg.senderId === this.userId,
                    timestamp: msg.createdDate,
                    status: msg.status,
                    images: msg.messageImageUrls
                }));
                this.allImages = this.$refs.chatbox.messages
                    .flatMap(msg => msg.images || []);
                this.markMessageAsRead();

            } catch (error) {
                console.error("Error fetching messages:", error);
            }

            this.$nextTick(() => {
                document.querySelector('.vd-chatbox').classList.add('show');
                document.querySelector('.vd-chatbox').classList.remove('minimize');
            });
        }
,
        closeChat() {
            if (this.selectedFriend) {
                this.connection.invoke("LeaveChatRoom", this.selectedFriend.chatRoomId);
                this.selectedFriend = null;
            }
            this.$nextTick(() => {
                document.querySelector('.vd-chatbox').classList.remove('show');
            });
        },
        toggleChatSize() {
            this.isMinimized = !this.isMinimized;

            // Cập nhật class CSS để hiển thị/thu gọn hộp chat
            this.$nextTick(() => {
                const chatbox = document.querySelector('.vd-chatbox');
                if (this.isMinimized) {
                    chatbox.classList.add('minimize'); // Thêm class thu gọn
                } else {
                    chatbox.classList.remove('minimize'); // Bỏ class thu gọn
                    this.$refs.chatbox.scrollToBottom(); // Cuộn xuống cuối danh sách khi mở lại
                }
            });
        }
        ,
        async sendMessage(content, files = null) {
            if (this.selectedFriend && (content || files)) {
                const fileMetadata = files
                    ? Array.from(files).map(async (file) => {
                        const base64Data = await this.readFileAsBase64(file);
                        return {
                            Name: file.name,
                            ContentType: file.type,
                            Data: base64Data
                        };
                    })
                    : [];

                const metadataList = await Promise.all(fileMetadata);

                const newMessage = {
                    id: null,
                    text: content || null,
                    isSent: true,
                    timestamp: Math.floor(Date.now() / 1000),
                    status: 0,
                    images: []
                };

                this.$refs.chatbox.messages.push(newMessage);

                try {
                    const serverResponse = await this.connection.invoke(
                        "SendMessageToGroup",
                        this.selectedFriend.chatRoomId,
                        this.userId,
                        content,
                        JSON.stringify(metadataList) // Gửi metadata JSON
                    );

                    if (serverResponse && serverResponse.messageId) {
                        newMessage.status = 1;
                        newMessage.id = serverResponse.messageId;
                        newMessage.images = serverResponse.messageImageUrls || [];
                        console.log("Message sent successfully:", newMessage);
                    } else {
                        throw new Error("Server did not return a valid message ID.");
                    }
                } catch (error) {
                    newMessage.status = -1;
                    console.error("Error sending message:", error);
                }
            }
        }
        ,
        handleImageClick({ imageUrl, index }) {
            this.openImageSlider(index);
        },
        openImageSlider(startIndex) {
            this.selectedImageIndex = startIndex;
            this.isImageSliderVisible = true;
            this.$nextTick(() => {
                // Ngăn cuộn trang
                document.body.style.overflow = 'hidden';
                window.addEventListener('keydown', this.handleKeydown);
                this.initSwiper();
            });
        },
        handleKeydown(event) {
            if (this.isImageSliderVisible && event.key === 'Escape') {
                this.closeImageSlider();
            }
        },
        closeImageSlider() {
            this.isImageSliderVisible = false;
            console.log(this.isImageSliderVisible)
            document.body.style.overflow = '';
            if (this.swiperInstance) {
                this.swiperInstance.destroy(true, true);
                this.swiperInstance = null;
            }
        },
        initSwiper() {
            this.swiperInstance = new Swiper('.swiper-container', {
                initialSlide: this.selectedImageIndex,
                slidesPerView: 1,
                spaceBetween: 100,
                navigation: {
                    nextEl: '.swiper-button-next',
                    prevEl: '.swiper-button-prev',
                },
                pagination: {
                    el: '.swiper-pagination',
                    clickable: true,
                    renderBullet: (index, className) => {
                        return `<img class="${className}" src="${this.allImages[index]}" alt="Thumbnail" />`;
                    }
                },
                keyboard: {
                    enabled: true,
                    onlyInViewport: true,
                },
            });
        },
        readFileAsBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.onload = () => resolve(reader.result);
                reader.onerror = (error) => reject(error);
                reader.readAsDataURL(file);
            });
        }

    }
});

// Chatbox component
Vue.component('chatbox', {
    template: `
<div class="vd-chatbox" :class="{ minimize: isMinimized }">
    <div class="vd-chat-header">
        <h2>Chat with {{ friend.friendFullName }}</h2>
        <div class="vd-chat-widget">
            <span class="vd-minimize-btn" @click="$emit('toggle-chat-size')"></span>
            <span class="vd-close-btn" @click="closeChat">&times;</span>
        </div>
    </div>
    <div class="vd-chat-messages" v-show="!isMinimized" ref="chatMessages">
        <div v-for="(message, index) in messages" :key="index"
             :class="['vd-message', message.isSent ? 'vd-sent' : 'vd-received']"
             :title="formatTime(message.timestamp)">
            <p v-if="message.text">{{ message.text }}</p>
            <div v-if="message.images && message.images.length" class="message-images">
                <img
                    v-for="(image, i) in message.images"
                    :key="i"
                    :src="image"
                    alt="Image"
                    class="message-image"
                    @click="emitImageClick(image, i)"
                >
            </div>


            <span v-if="message.isSent && (index === lastSentMessageIndex || message.status === -1)" class="message-status">
                <span v-if="message.status === 0">Đang gửi...</span>
                <span v-else-if="message.status === 1">Đã gửi</span>
                <span v-else-if="message.status === 2 && shouldShowReadStatus">Đã đọc</span>
                <span v-else-if="message.status === -1">Lỗi</span>
            </span>
        </div>
    </div>
    <div class="vd-chat-input" v-show="!isMinimized">
        <textarea v-model="newMessage" placeholder="Nhập tin nhắn..."
                  @input="adjustTextareaHeight" @keydown="handleKeyDown" rows="1"></textarea>
        <div class="file-upload">
            <label for="fileInput">
                <span><i class="fa-solid fa-image"></i></span> <!-- Icon or placeholder for file upload -->
            </label>
            <input type="file" id="fileInput" @change="handleFileInput" multiple hidden>
            <div v-if="selectedFiles.length" class="selected-files">
                <span v-for="(file, index) in selectedFiles" :key="index">{{ file.name }}</span>
            </div>
        </div>
        <button @click="sendMessage">Gửi</button>
    </div>
</div>
    `,
    props: ['friend', 'isMinimized', 'messages'],
    data() {
        return {
            newMessage: '',
            selectedFiles: [], // Lưu trữ danh sách tệp được chọn
            isScrollLocked: true, // Khóa cuộn khi tải dữ liệu
            isImageModalVisible: false, // Trạng thái hiển thị modal
            selectedImageUrl: '' // URL ảnh được chọn để xem
        };
    },
    watch: {
        isMinimized(newVal) {
            if (!newVal) {
                this.scrollToBottom(); // Cuộn xuống cuối danh sách khi mở lại
            }
        },
        messages: {
            handler(newMessages, oldMessages) {
                // Chỉ cuộn khi gửi tin nhắn mới
                if (!this.isScrollLocked) {
                    this.scrollToBottom();
                }
            },
            deep: true
        }
    },
    computed: {
        shouldShowReadStatus() {
            const lastSentIndex = this.lastSentMessageIndex;
            const lastReadMessage = this.messages
                .filter((msg) => msg.status === 2 && msg.isSent) // Tìm tin nhắn cuối cùng được đọc
                .pop();

            if (!lastReadMessage) {
                return false; // Không có tin nhắn được đọc
            }

            const lastReadIndex = this.messages.indexOf(lastReadMessage);

            // Nếu người nhận đã gửi tin nhắn sau tin nhắn được đọc, ẩn trạng thái "Đã đọc"
            return this.messages
                .slice(lastReadIndex + 1)
                .every((msg) => msg.isSent); // Chỉ hiển thị nếu không có tin nhắn nhận nào sau đó
        },
        lastSentMessageIndex() {
            // Tìm chỉ số của tin nhắn gửi cuối cùng
            return this.messages
                .map((msg, index) => ({ isSent: msg.isSent, index }))
                .filter(msg => msg.isSent)
                .map(msg => msg.index)
                .pop(); // Lấy tin nhắn cuối cùng
        }
    },
    methods: {
        emitImageClick(imageUrl, index) {
            this.$emit('image-clicked', { imageUrl, index }); // Truyền index cùng URL
        },

        handleFileInput(event) {
            const files = Array.from(event.target.files); // Lấy danh sách file từ input
            const validFiles = []; // Danh sách file hợp lệ
            const maxFileSize = 100 * 1024 * 1024; 

            const allowedMimeTypes = [
                'image/jpeg',  // JPEG
                'image/jpg',   // JPG
                'image/png',   // PNG
                'image/gif',   // GIF
                'image/bmp',   // BMP
                'image/webp',  // WebP
                'image/svg+xml', // SVG
                'image/tiff',  // TIFF
                'image/x-icon', // ICO
            ];

            files.forEach(file => {
                if (allowedMimeTypes.includes(file.type) && file.size <= maxFileSize) {
                    validFiles.push(file); // Chỉ thêm file hợp lệ
                } else {
                    console.error(`File "${file.name}" không hợp lệ hoặc vượt quá kích thước cho phép.`);
                }
            });

            this.selectedFiles = validFiles; // Lưu các file hợp lệ
            console.log("Danh sách file được chọn:", this.selectedFiles);
        },
        formatTime(timestamp) {
            const messageDate = new Date(timestamp * 1000);
            const now = new Date();
            const timeDifference = now - messageDate;
            const oneDay = 24 * 60 * 60 * 1000;
            const oneWeek = 7 * oneDay;

            if (timeDifference < oneDay && messageDate.getDate() === now.getDate()) {
                return messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
            } else if (timeDifference < oneWeek) {
                const dayNames = ["Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy"];
                const dayOfWeek = dayNames[messageDate.getDay()];
                return `${dayOfWeek} ${messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
            } else {
                return messageDate.toLocaleString("vi-VN", {
                    weekday: 'long',
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });
            }
        },
        sendMessage() {
            const message = this.newMessage.trim();
            if (message || this.selectedFiles.length) {
                this.isScrollLocked = false; // Bật cuộn khi gửi tin nhắn mới
                this.$emit('send-message', message, this.selectedFiles); // Gửi tin nhắn và file lên cha
                this.newMessage = '';
                this.selectedFiles = []; // Xóa file sau khi gửi
                this.resetTextareaHeight();
            }
        },
        adjustTextareaHeight(event) {
            const textarea = this.$el.querySelector('textarea');
            textarea.style.height = 'auto';
            textarea.style.height = `${textarea.scrollHeight}px`;

            if (textarea.scrollHeight > 150) {
                textarea.style.overflowY = 'auto';
            } else {
                textarea.style.overflowY = 'hidden';
            }
        },
        resetTextareaHeight() {
            const textarea = this.$el.querySelector('textarea');
            textarea.style.height = 'auto';
            textarea.style.overflowY = 'hidden';
        },
        handleKeyDown(event) {
            if (event.key === "Enter" && !event.shiftKey) {
                event.preventDefault();
                this.sendMessage();
            }
        },
        closeChat() {
            this.$emit('close-chat');
        },
        scrollToBottom() {
            // Cuộn xuống cuối danh sách tin nhắn
            this.$nextTick(() => {
                const chatMessages = this.$refs.chatMessages;
                if (chatMessages) {
                    chatMessages.scrollTop = chatMessages.scrollHeight;
                }
            });
        }
    },

    mounted() {
        // Khi component được mount, đặt tin nhắn cuối cùng ở vị trí hiện tại
        const chatMessages = this.$refs.chatMessages;
        if (chatMessages) {
            const latestMessage = chatMessages.lastElementChild;
            if (latestMessage) {
                latestMessage.scrollIntoView({ block: "nearest" });
            }
        }
        this.isScrollLocked = false; // Mở khóa cuộn sau khi tải
    }
});
