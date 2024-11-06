new Vue({
    el: '#home-app',
    data: {
        posts: [],
        postReactions: [],
        commentReactions: [],
        selectedPost: {},
        comments: [],
        commentsNotBuild: [],
        newComment: '',
        replyTarget: null,
        newPostContent: '',
        userId: userLogged,
        newPostFor: 0
    },
    created() {
        this.loadPosts();
    },
    methods: {
        async postingPost() {
            // Use Moment.js to get the current time in VN timezone (UTC+7) with milliseconds
            const createdDate = moment().utcOffset(7).format('YYYY-MM-DDTHH:mm:ss.SSSZ'); // ISO-like format with milliseconds and UTC+7

            const postData = {
                UserId: this.userId, // ID người dùng
                PostId: null, // Nếu muốn cập nhật bài viết hiện có, có thể thay đổi
                Content: this.newPostContent, // Nội dung bài viết
                CreatedDate: createdDate, // Use moment.js to format the current time
                PostFor: parseInt(this.newPostFor) // Chọn mức độ hiển thị (0, 1, 2)
            };

            try {
                console.log(postData);
                // Gửi yêu cầu POST tới controller trong backend
                const response = await fetch('/Post/AddNew', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json charset=utf-8' // Define the content type
                    },
                    body: JSON.stringify(postData) // Send the postData as JSON
                });

                // Kiểm tra phản hồi từ server
                const data = await response.json();

                if (response.ok && data.success) {
                    // Nếu thành công, tải lại danh sách bài viết
                    this.loadPosts();

                    // Đặt lại trạng thái các biến sau khi đăng thành công
                    this.newPostContent = '';
                    this.newPostFor = '0'; // Đặt lại mức độ hiển thị về mặc định là công khai
                } else {
                    // Xử lý lỗi nếu bài viết không được đăng thành công
                    console.error('Lỗi khi đăng bài:', data.message || 'Có lỗi xảy ra');
                }
            } catch (error) {
                // Xử lý lỗi nếu có vấn đề trong quá trình gửi yêu cầu
                console.error('Lỗi khi gửi yêu cầu:', error);
            }
        },
        async sharePost(post) {
            // Use Moment.js to get the current time in VN timezone (UTC+7) with milliseconds
            const createdDate = moment().utcOffset(7).format('YYYY-MM-DDTHH:mm:ss.SSSZ'); // ISO-like format with milliseconds and UTC+7

            const postData = {
                UserId: this.userId, // ID người dùng
                PostId: post.id, // Nếu muốn cập nhật bài viết hiện có, có thể thay đổi
                CreatedDate: createdDate, // Use moment.js to format the current time
            };

            try {
                console.log(postData);
                const response = await fetch('/Post/ShareAndAddNew', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json charset=utf-8' // Define the content type
                    },
                    body: JSON.stringify(postData) // Send the postData as JSON
                });

                // Kiểm tra phản hồi từ server
                const data = await response.json();

                if (response.ok && data.success) {
                    // Nếu thành công, tải lại danh sách bài viết
                    this.loadPosts();

                    // Đặt lại trạng thái các biến sau khi đăng thành công
                    this.newPostContent = '';
                    this.newPostFor = '0'; // Đặt lại mức độ hiển thị về mặc định là công khai
                } else {
                    // Xử lý lỗi nếu bài viết không được đăng thành công
                    console.error('Lỗi khi đăng bài:', data.message || 'Có lỗi xảy ra');
                }
            } catch (error) {
                // Xử lý lỗi nếu có vấn đề trong quá trình gửi yêu cầu
                console.error('Lỗi khi gửi yêu cầu:', error);
            }
        },
        async loadPosts() {
            try {
                const response = await fetch('/Post/GetAll');
                const data = await response.json();

                this.posts = data.apiData.map(post => {
                    // Convert Unix timestamps to JS Date objects for display
                    const createdDate = new Date(post.createdDate * 1000);
                    post.displayDate = this.getRelativeTime(createdDate);

                    // Fetch reactions for the post and store them in postReactions
                    this.getPostReactions(post);

                    return post;

                });
            } catch (error) {
                console.error('Error fetching posts:', error);
            }
        },
        async getPostReactions(post) {
            try {
                const response = await fetch(`/Post/GetReactionByPost?postid=${post.id}`);
                const data = await response.json();

                // Add reactions to the postReactions array for this specific post
                this.$set(this.postReactions, post.id, data.apiData);

                if (!post.hasOwnProperty('likedByCurrentUser')) {
                    Vue.set(post, 'likedByCurrentUser', false); // Default to false if not set
                }

                // Check if the current user has reacted to this post within postReactions[post.id]
                if (this.postReactions[post.id] != null) {
                    post.likedByCurrentUser = this.postReactions[post.id].some(reaction => reaction.userId === this.userId);
                } else {
                    post.likedByCurrentUser = false
                }

            } catch (error) {
                console.error('Error fetching reactions:', error);
            }
        },
        async likePost(post) {
            const postData = {
                UserId: this.userId,
                PostId: post.id,
                type: post.likedByCurrentUser ? 0 : 1 // 0 = unlike, 1 = like
            };

            try {
                const response = await fetch('/Post/ReactionPost', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json charset=utf-8'
                    },
                    body: JSON.stringify(postData)
                });

                const data = await response.json();

                if (response.ok && data.success) {
                    post.likedByCurrentUser = !post.likedByCurrentUser; // Toggle the like status

                    // Refresh the reactions after like/unlike
                    this.loadPosts();
                } else {
                    console.error('Error liking post:', data.message || 'An error occurred');
                }
            } catch (error) {
                console.error('Error sending like request:', error);
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
        loadComments(postId) {
            fetch(`/Comment/GetCommentByPostId/${postId}`)
                .then(response => response.json())
                .then(data => {
                    this.commentsNotBuild = data.apiData;
                    this.comments = data.apiData.map(comment => {
                        const createdDate = new Date(comment.createdDate * 1000);
                        comment.displayDate = this.getRelativeTime(createdDate);


                        this.getCommentReactions(comment);
                        return comment;
                        console.log(comment);
                    });
                    // Sau khi xử lý thời gian, xây dựng cấu trúc bình luận
                    this.comments = this.buildCommentThread(this.comments);
                })
                .catch(error => console.error('Error fetching comments:', error));
        },
        async getCommentReactions(comment) {
            try {
                const response = await fetch(`/Comment/GetReactionByComment?commentid=${comment.id}`);
                const data = await response.json();
                console.log(data.apiData);
                this.$set(this.commentReactions, comment.id, data.apiData);


                if (!comment.hasOwnProperty('likedByCurrentUser')) {
                    Vue.set(comment, 'likedByCurrentUser', false);
                }

                if (this.commentReactions[comment.id] != null) {
                    comment.likedByCurrentUser = this.commentReactions[comment.id].some(reaction => reaction.userId === this.userId);
                } else {
                    comment.likedByCurrentUser = false
                }

            } catch (error) {
                console.error('Error fetching reactions:', error);
            }
        }
        ,
        showComments(post) {
            this.selectedPost = post;
            this.loadComments(post.id);
            $('#commentsModal').modal('show');
        },
        buildCommentThread(comments) {
            const MAX_DEPTH = 2;
            const map = {};
            const result = [];

            comments.forEach(comment => {
                comment.childrenArray = [];
                comment.depth = 0;
                map[comment.id] = comment;
                comment.expanded = false;
            });

            comments.forEach(comment => {
                if (comment.commentId !== null) {
                    const parent = map[comment.commentId];
                    if (parent) {
                        comment.depth = parent.depth + 1;

                        if (parent.depth < MAX_DEPTH) {
                            parent.childrenArray.push(comment);
                        } else {
                            let ancestor = parent;
                            while (ancestor.depth >= MAX_DEPTH) {
                                ancestor = map[ancestor.commentId];
                            }
                            comment.depth = MAX_DEPTH;
                            ancestor.childrenArray.push(comment);
                        }
                    }
                } else {
                    result.push(comment);
                }
            });

            return result;
        },

        async likeComment(commentId) {
            const comment = this.commentsNotBuild.find(c => c.id === commentId);
            if (!comment) {
                console.error('Comment not found!');
                return;
            }

            const commentData = {
                UserId: this.userId,
                CommentId: comment.id,
                type: comment.likedByCurrentUser ? 0 : 1, // 0 = unlike, 1 = like
            };

            try {
                const response = await fetch('/Comment/ReactionComment', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json charset=utf-8'
                    },
                    body: JSON.stringify(commentData)
                });

                const data = await response.json();

                // Toggle the like state for the comment
                comment.likedByCurrentUser = !comment.likedByCurrentUser;
                comment.reactCount += comment.likedByCurrentUser ? 1 : -1;
            } catch (error) {
                console.error('Error sending like request:', error);
            }
        },
        replyToComment(comment) {
            this.replyTarget = comment;  // Set the comment being replied to
            this.newComment = '@@' + `${comment.fullName} `;  // Optional: Pre-fill the input with the commenter's name
            this.$nextTick(() => {
                // Ensure the input field is focused after being rendered
                const inputField = document.querySelector('.add-comment input');
                if (inputField) inputField.focus();
            });
        },
        async addComment() {
            if (!this.newComment.trim()) {
                alert('Please enter a comment.');
                return;
            }
            const createdDate = moment().utcOffset(7).format('YYYY-MM-DDTHH:mm:ss.SSSZ');

            const newCommentData = {
                userId: this.userId,
                content: this.newComment,
                postId: this.selectedPost.id,
                commentId: this.replyTarget ? this.replyTarget.id : null,  // Use replyTarget if replying
                CreatedDate: createdDate,
            };

            try {
                const response = await fetch('/Comment/AddNew', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(newCommentData),
                });
                const data = await response.json();

                if (data.success) {
                    this.loadComments(this.selectedPost.id);
                    this.newComment = '';  // Clear the comment input
                    this.replyTarget = null;  // Reset the reply target after submitting
                    console.log('Comment added successfully');
                } else {
                    console.error('Error adding comment:', data.message || 'Failed to add comment.');
                }
            } catch (error) {
                console.error('Error submitting comment:', error);
            }
        }
    }
});


Vue.component('comment-item', {
    props: ['comment'],
    data() {
        return {
            expanded: this.comment.expanded || false,
        };
    },
    methods: {
        handleLike() {
            this.$emit('like', this.comment.id);
        },
        toggleComments() {
            this.expanded = !this.expanded;
        }
    },
    template: `
    <div class="comment-item" :class="{ 'depth-0': comment.depth === 0 }">
        <div class="comment-container">
            <div v-if="comment.depth > 0" class="comment-border"></div>
            <div class="comment-body" :style="{ paddingLeft: comment.depth === 0 ? '0px' : (40 * comment.depth + 'px') }">
                <figure class="mr-2">
                    <img src="/images/resources/admin.jpg" alt="User Avatar" class="avatar rounded-circle">
                </figure>
                <div>
                    <div class="comment-content">
                        <strong>{{ comment.fullName }}</strong>
                        <p class="mb-1">{{ comment.content }}</p>
                    </div>
                    <div class="comment-actions">
                        <span v-on:click="handleLike">
                          <!-- Use the active class to visually show if the comment is liked by the current user -->
                          <span class="like-comment" :class="{ 'active': comment.likedByCurrentUser }">
                             Thích
                          </span>
                        </span>
                        <span v-on:click="$emit('reply', comment)">Phản hồi</span>
                        <span class="timestamp">{{ comment.displayDate }}</span>
                        <span class="react-comment-count" style="margin-right: 5px;">{{ comment.reactCount }} thích</span>
                    </div>

                    <!-- Show reply input if this comment is the reply target -->
                    <div v-if="comment.id === $parent.replyTarget?.id" class="add-comment d-flex align-items-center mt-3 p-3 rounded bg-light">
                        <figure class="mr-2">
                            <img src="/images/resources/admin.jpg" alt="Your Avatar" class="avatar rounded-circle">
                        </figure>
                        <div class="box-input position-relative w-100">
                            <input type="text" v-model="$parent.newComment" placeholder="Viết câu trả lời..." class="form-control custom-input rounded-pill px-4">
                            <div class="input-icons d-flex align-items-center position-absolute">
                                <i class="far fa-smile"></i>
                                <i class="fas fa-camera"></i>
                                <i class="fas fa-gif"></i>
                                <i class="fas fa-sticky-note"></i>
                            </div>
                        </div>
                        <button class="btn btn-light ml-2" v-on:click="$parent.addComment" v-on:keyup.enter="$parent.addComment" v-bind:disabled="!$parent.newComment.trim()">
                            <i class="fas fa-paper-plane"></i>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Show replies only if expanded -->
        <div v-if="comment.childrenArray && comment.childrenArray.length > 0">
            <div v-if="expanded" class="comment-replies">
                <comment-item v-for="child in comment.childrenArray"
                              :key="child.id"
                              :comment="child"
                              v-on:reply="$emit('reply', child)"
                              v-on:like="$emit('like', child.id)">
                </comment-item>
            </div>

            <div class="see-more-comments-container">
                <div v-if="comment.depth < 2" class="comment-border" :class="{ 'depth-0': comment.depth === 0, 'depth-1': comment.depth === 1 }"></div>
                <button v-if="!expanded" v-on:click="toggleComments" class="see-more-comments" :class="{ 'depth-0': comment.depth === 0, 'depth-1': comment.depth === 1 }">See More Comments</button>
                <button v-if="expanded" v-on:click="toggleComments" class="see-more-comments" :class="{ 'depth-0': comment.depth === 0, 'depth-1': comment.depth === 1 }">Hide Comments</button>
            </div>
        </div>
    </div>
    `,
});
