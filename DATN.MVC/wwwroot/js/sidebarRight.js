new Vue({
    el: '#sidebar-app',
    data: {
        friends: [],
        selectedFriend: {},
        userId: userLogged,
        newPostFor: 0
    },
    created() {


        this.loadFriendOfUser();
    },
    methods: {
        async loadFriendOfUser() {
            const userData = {
                UserId: this.userId,
                Status: 1
            };
            console.log(userData);
            try {
                const response = await fetch(`/Friend/GetListFriendOfUser`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json charset=utf-8'
                    },
                    body: JSON.stringify(userData)
                });

                const data = await response.json();
                this.friends = data.apiData;
                console.log(this.friends);
                
            } catch (error) {
                console.error('Error fetching reactions:', error);
            }
        },
        getRelativeTime(date) {
            const now = new Date();
            const rtf = new Intl.RelativeTimeFormat('vi', { numeric: 'auto' });
            const diffInSeconds = Math.floor((now - date) / 1000);
            const diffInMinutes = Math.floor(diffInSeconds / 60);
            const diffInHours = Math.floor(diffInMinutes / 60);
            const diffInDays = Math.floor(diffInHours / 24);
            const diffInMonths = Math.floor(diffInDays / 30);

            if (diffInSeconds < 60) {
                return rtf.format(-diffInSeconds, 'second');
            } else if (diffInMinutes < 60) {
                return rtf.format(-diffInMinutes, 'minute');
            } else if (diffInHours < 24) {
                return rtf.format(-diffInHours, 'hour');
            } else if (diffInDays < 30) {
                return rtf.format(-diffInDays, 'day');
            } else {
                return date.toLocaleDateString('vi-VN', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit'
                });
            }
        },
        openChatBox(friend) {
            this.selectedFriend = friend;
            console.log(this.selectedFriend);
            //this.loadCommets(post.id);
            $('.chat-box').addClass("show");
        }
    }
});
