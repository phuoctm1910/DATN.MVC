namespace DATN.MVC.Models
{
    public class WebRTCModel
    {
        public class RTCSessionDescription
        {
            public string Type { get; set; } // "offer" hoặc "answer"
            public string Sdp { get; set; } // SDP của WebRTC
        }

        public class RTCIceCandidate
        {
            public string Candidate { get; set; } // Chuỗi ICE candidate
            public string SdpMid { get; set; } // ID phương tiện (Media stream identifier)
            public int? SdpMLineIndex { get; set; } // Index dòng M-line
        }
    }
}
