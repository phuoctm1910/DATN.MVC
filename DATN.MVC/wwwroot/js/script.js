$(window).load(function () {
    "use strict";
    $('.wavy-wraper').addClass('hidden');
});
$(document).ready(function ($) {
    "use strict";
    function delay() {
        $(".popup-wraper.subscription").fadeIn();
    }
    window.setTimeout(delay, 3000);
    $('.popup-closed').on('click', function () {
        $('.popup-wraper.subscription').addClass('closed');
        return false;
    });
    $(window).scroll(function () {
        if ($(this).scrollTop() > 150) {
            $('.recent-posts').show(100);
        } else {
            $('.recent-posts').hide(100);
        }
    });
    $('.top-area > .setting-area > li > a').on("click", function () {
        var $parent = $(this).parent('li');

        if ($(this).attr('title') !== 'Home') {
            $(this).addClass('active').parent().siblings().children('a').removeClass('active');
        }

        $parent.siblings().children('div').removeClass('active');
        $(this).siblings('div').toggleClass('active');

        return false;
    });

    $("body *").not('.top-area > .setting-area > li > a').on("click", function () {
        $(".top-area > .setting-area > li > a").removeClass('active');
    });
    $(".new-postbox").click(function () {
        $(".postbox").css('z-index', '99');
        $(".postoverlay").fadeIn(500);
    });
    $(".postoverlay").not(".new-postbox").click(function () {
        $(".postbox").css('z-index', '');
        $(".postoverlay").fadeOut(500);
    });
    $("[type = submit]").click(function () {
        var post = $("textarea").val();
        $("<p class='post'>" + post + "</p>").appendTo("section");
    });
    $('.main-menu > span').on('click', function () {
        $('.nav-list').slideToggle(300);
    });
    $('.comment').on('click', function () {
        $(this).parents(".post-meta").siblings(".coment-area").slideToggle("slow");
    });
    $('.add-loc').on('click', function () {
        $('.add-location-post').slideToggle("slow");
    });
    $('.from-gallery').on('click', function () {
        $('.already-gallery').addClass('active');
    });
    $('.canceld').on('click', function () {
        $('.already-gallery').removeClass('active');
    });
    $('.story-box').on('click', function () {
        $('.stories-wraper').addClass('active');
    });
    $('.close-story').on('click', function () {
        $('.stories-wraper').removeClass('active');
    });
    $('.edit-prof').on('click', function () {
        $('.popup-wraper').addClass('active');
    });
    $('.popup-closed').on('click', function () {
        $('.popup-wraper, .popup-wraper1').removeClass('active');
    });
    $('.item-upload').on('click', function () {
        $('.popup-wraper4').addClass('active');
    });
    $('.popup-closed').on('click', function () {
        $('.popup-wraper4').removeClass('active');
    });
    $('.item-upload.album').on('click', function () {
        $('.popup-wraper5').addClass('active');
    });
    $('.popup-closed').on('click', function () {
        $('.popup-wraper5').removeClass('active');
    });
    $('.event-title h4').on('click', function () {
        $('.popup-wraper7').addClass('active');
    });
    $('.popup-closed').on('click', function () {
        $('.popup-wraper7').removeClass('active');
    });
    $('.msg-pepl-list .nav-item').on('click', function () {
        $(this).removeClass('unread');
    });
    $('.select-gender > li').click(function () {
        $(this).addClass('selected').siblings().removeClass('selected');
    });
    $('.amount-select > li').click(function () {
        $(this).addClass('active').siblings().removeClass('active');
    });
    $('.pay-methods > li').click(function () {
        $(this).addClass('active').siblings().removeClass('active');
    });
    $('.user-add').on('click', function () {
        $('.popup-wraper6').addClass('active');
    });
    $('.popup-closed').on('click', function () {
        $('.popup-wraper6').removeClass('active');
        return false;
    });
    $('.send-mesg').on('click', function () {
        $('.popup-wraper1').addClass('active');
        return false;
    });
    $('.bad-report').on('click', function () {
        $('.popup-wraper3').addClass('active');
        return false;
    });
    $('.popup-closed, .cancel').on('click', function () {
        $('.popup-wraper3').removeClass('active');
        return false;
    });
    jQuery(window).on("load", function () {
        $('.show-comt').bind('click', function () {
            $('.pit-comet-wraper').addClass('active');
        });
    });
    $('.add-pitrest > a, .pitred-links > .main-btn, .create-pst').on('click', function () {
        $('.popup-wraper').addClass('active');
        return false;
    });
    $('.share-pst').on('click', function () {
        $('.popup-wraper2').addClass('active');
        return false;
    });
    $('.popup-closed, .cancel').on('click', function () {
        $('.popup-wraper2').removeClass('active');
    });
    $('.audio-call').on('click', function () {
        $('.call-wraper').addClass('active');
    });
    $('.decline-call, .later-rmnd').on('click', function () {
        $('.call-wraper').removeClass('active');
    });
    $('.video-call').on('click', function () {
        $('.vid-call-wraper').addClass('active');
    });
    $('.decline-call, .later-rmnd').on('click', function () {
        $('.vid-call-wraper').removeClass('active');
    });
    if ($.isFunction($.fn.TouchSpin)) {
        $('.qty').TouchSpin({});
    }
    $(init);
    function init() {
        $(".droppable-area1, .droppable-area2").sortable({
            connectWith: ".connected-sortable",
            stack: '.connected-sortable ul'
        }).disableSelection();
    }

    $('.search-data').on('click', function () {
        $(".searchees").fadeIn("slow", function () { });
        return false;
    });
    $('.cancel-search').on('click', function () {
        $(".searchees").fadeOut("slow", function () { });
        return false;
    });
    $("body *").not('.top-area > .setting-area > li > a').on("click", function () {
        $(".top-area > .setting-area > li > div").not('.searched').removeClass('active');
    });
    $('.user-img').on('click', function () {
        $('.user-setting').toggleClass("active");
    });
    $('.friendz-list > li, div > .chat-users > li, .drops-menu > li > a.show-mesg').on('click', function () {
        $('.chat-box').addClass("show");
        return false;
    });

    $('.close-mesage').on('click', function () {
        $('.chat-box').removeClass("show");
        return false;
    });
    if ($.isFunction($.fn.perfectScrollbar)) {
        $('.dropdowns, .twiter-feed, .invition, .followers, .chatting-area, .peoples, #people-list, .chat-list > ul, .message-list, .chat-users, .left-menu, .sugestd-photo-caro, .popup.events, .related-tube-psts, .music-list, .more-songs, .media > ul, .conversations, .msg-pepl-list, .menu-slide, .frnds-stories, .modal-body .we-comet').perfectScrollbar();
    }
    $('.trigger').on("click", function () {
        $(this).parent(".menu").toggleClass("active");
    });
    $('.menu-small').on("click", function () {
        $(".fixed-sidebar.left").addClass("open");
    });
    $('.closd-f-menu').on("click", function () {
        $(".fixed-sidebar.left").removeClass("open");
    });
    $('.add-smiles > span, .smile-it').on("click", function () {
        $(this).siblings(".smiles-bunch").toggleClass("active");
    });
    $('.smile-it').on("click", function () {
        $(this).children(".smiles-bunch").toggleClass("active");
    });
    $('.save-post, .bane, .get-link').on("click", function () {
        $(this).toggleClass("save");
    });
    $('.notification-box > ul li > i.del').on("click", function () {
        $(this).parent().slideUp();
        return false;
    });
    $('.f-page > figure i').on("click", function () {
        $(".drop").toggleClass("active");
    });
    $('.sugestd-photo-caro > li').on('click', function () {
        $(this).toggleClass('active');
        return false;
    });
    $('.minus').click(function () {
        var $input = $(this).parent().find('input');
        $('.minus').on("click", function () {
            $(this).siblings('input').removeClass("active");
            $(this).siblings('.plus').removeClass("active");
        });
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 0 : count;
        $input.val(count);
        $input.change();
        return false;
    });
    $('.plus').click(function () {
        var $input = $(this).parent().find('input');
        $('.plus').on("click", function () {
            $(this).addClass("active");
            $(this).siblings('input').addClass("active");
        });
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        return false;
    });
    $(".get-link").click(function (event) {
        event.preventDefault();
        CopyToClipboard("This is some test value.", true, "Link copied");
    });

    $(".content").each(function () {
        var contentElement = $(this);
        var originalContent = contentElement.html();

        var lineHeight = parseInt(contentElement.css('line-height'), 10);
        var maxHeight = lineHeight * 3; // Limit to 4 lines

        if (contentElement[0].scrollHeight > maxHeight) {
            contentElement.css("max-height", maxHeight + "px");

            contentElement.after('<div class="see-more">Xem thêm</div>');

            contentElement.next(".see-more").on("click", function () {
                if (contentElement.hasClass("expanded")) {
                    contentElement.removeClass("expanded").css("max-height", maxHeight + "px");
                    $(this).text("Xem thêm");
                } else {
                    contentElement.addClass("expanded").css("max-height", "none");
                    $(this).text("Ẩn bớt");
                }
            });
        }
    });



    function CopyToClipboard(value, showNotification, notificationText) {
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val(value).select();
        document.execCommand("copy");
        $temp.remove();
        if (typeof showNotification === 'undefined') {
            showNotification = true;
        }
        if (typeof notificationText === 'undefined') {
            notificationText = "Copied to clipboard";
        }
        var notificationTag = $("div.copy-notification");
        if (showNotification && notificationTag.length == 0) {
            notificationTag = $("<div/>", {
                "class": "copy-notification",
                text: notificationText
            });
            $("body").append(notificationTag);
            notificationTag.fadeIn("slow", function () {
                setTimeout(function () {
                    notificationTag.fadeOut("slow", function () {
                        notificationTag.remove();
                    });
                }, 1000);
            });
        }
    }

    (function ($) {
        jQuery.expr[':'].Contains = function (a, i, m) {
            return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
        }
            ;
        function listFilter(searchDir, list) {
            var form = $("<form>").attr({
                "class": "filterform",
                "action": "#"
            })
                , input = $("<input>").attr({
                    "class": "filterinput",
                    "type": "text",
                    "placeholder": "Search Contacts..."
                });
            $(form).append(input).appendTo(searchDir);
            $(input).change(function () {
                var filter = $(this).val();
                if (filter) {
                    $(list).find("li:not(:Contains(" + filter + "))").slideUp();
                    $(list).find("li:Contains(" + filter + ")").slideDown();
                } else {
                    $(list).find("li").slideDown();
                }
                return false;
            }).keyup(function () {
                $(this).change();
            });
        }
        $(function () {
            listFilter($("#searchDir"), $("#people-list"));
        });
    }(jQuery));
    $('body').show();
    NProgress.start();
    setTimeout(function () {
        NProgress.done();
        $('.fade').removeClass('out');
    }, 5000);
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
        $('[data-toggle="popover"]').popover();
    });
    if ($(window).width() < 981) {
        $(".sidebar").children().removeClass("stick-widget");
    }
    if ($.isFunction($.fn.stick_in_parent)) {
        $('.stick-widget').stick_in_parent({
            parent: '#page-contents',
            offset_top: 60,
        });
        $('.stick').stick_in_parent({
            parent: 'body',
            offset_top: 0,
        });
    }
    $(".we-page-setting").on("click", function () {
        $(".wesetting-dropdown").toggleClass("active");
    });
    $('#nightmode').on('change', function () {
        if ($(this).is(':checked')) {
            // Enable dark theme
            $('#dark-theme').removeAttr('disabled');
        } else {
            // Disable dark theme
            $('#dark-theme').attr('disabled', 'disabled');
        }
    });


    if ($.isFunction($.fn.chosen)) {
        $("select").chosen();
    }
    if ($.isFunction($.fn.userincr)) {
        $(".manual-adjust").userincr({
            buttonlabels: {
                'dec': '-',
                'inc': '+'
            },
        }).data({
            'min': 0,
            'max': 20,
            'step': 1
        });
    }
    
    (function () {
        // Initially hide half of the posts
        const posts = $('.loadMore .central-meta.item');
        let totalPosts = posts.length;
        let visiblePosts = 2;
        posts.slice(visiblePosts).css('display', 'none');

        // Function to load more posts
        function loadMorePosts() {
            if (visiblePosts >= totalPosts) {
                return; // All posts are already visible, do nothing
            }

            // Show loader
            $(".loader").css('display', 'block');

            setTimeout(function () {
                // Hide loader
                $(".loader").css('display', 'none');

                // Show hidden posts
                posts.slice(visiblePosts, visiblePosts + 2).css('display', 'inline-block');
                visiblePosts += 2;
            }, 2000); // Delay of 2 seconds for demonstration purposes
        }

        // Auto load more posts when scrolling reaches the bottom of .loadMore
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(".loadMore").height()) {
                loadMorePosts();
            }
        });
    })();


    if ($.isFunction($.fn.owlCarousel)) {
        $('.new-pod-post').owlCarousel({
            items: 3,
            loop: true,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });
        $('.recent-pods').owlCarousel({
            loop: true,
            margin: 20,
            smartSpeed: 1000,
            responsiveClass: true,
            nav: false,
            dots: true,
            responsive: {
                0: {
                    items: 1,
                    nav: false,
                    dots: false
                },
                600: {
                    items: 2,
                    nav: false
                },
                1000: {
                    items: 3,
                    nav: false,
                }
            }
        });
        $('.books-caro').owlCarousel({
            items: 5,
            loop: true,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 4,
                },
                1000: {
                    items: 5,
                }
            }
        });
        $('.suggested-caro').owlCarousel({
            items: 3,
            loop: true,
            margin: 30,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });
        $('.sponsor-logo').owlCarousel({
            items: 6,
            loop: true,
            margin: 30,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 3,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 6,
                }
            }
        });
        $('.contributorz').owlCarousel({
            items: 6,
            loop: true,
            margin: 20,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 6,
                }
            }
        });
        $('.suggested-frnd-caro').owlCarousel({
            items: 4,
            loop: true,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 4,
                },
                1000: {
                    items: 4,
                }
            }
        });
        $('.frndz-list').owlCarousel({
            items: 5,
            loop: true,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 5,
                }
            }
        });
        $('.photos-list').owlCarousel({
            items: 5,
            loop: true,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 5,
                }
            }
        });
        $('.videos-list').owlCarousel({
            items: 3,
            loop: true,
            margin: 30,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });
        $('.badge-caro').owlCarousel({
            items: 6,
            loop: true,
            margin: 30,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: true,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 4,
                },
                1000: {
                    items: 6,
                }
            }
        });
        $('.related-groups').owlCarousel({
            items: 6,
            loop: true,
            margin: 50,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                    margin: 10,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 6,
                }
            }
        });
        $('.pitred-trendings.six').owlCarousel({
            items: 6,
            loop: true,
            margin: 20,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                    margin: 10,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 6,
                }
            }
        });
        $('.pitred-trendings').owlCarousel({
            items: 4,
            loop: true,
            margin: 20,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                    margin: 10,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 4,
                }
            }
        });
        $('.succes-people').owlCarousel({
            items: 1,
            loop: true,
            margin: 0,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: true,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });
        $('.soundnik-featured').owlCarousel({
            items: 1,
            loop: true,
            margin: 0,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: true,
            center: false,
            animateOut: 'fadeOut',
            animateIn: 'fadein',
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });
        $('.promo-caro').owlCarousel({
            items: 3,
            loop: true,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,
                },
                600: {
                    items: 2,
                },
                1000: {
                    items: 3,
                }
            }
        });
        $('.testi-caro').owlCarousel({
            items: 1,
            loop: true,
            margin: 0,
            autoplay: true,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: false,
            dots: false,
            center: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,
                },
                600: {
                    items: 1,
                },
                1000: {
                    items: 1,
                }
            }
        });
        $('.text-caro').owlCarousel({
            items: 1,
            loop: true,
            margin: 0,
            autoplay: true,
            autoplayTimeout: 2500,
            autoplayHoverPause: true,
            dots: false,
            nav: false,
            animateIn: 'fadeIn',
            animateOut: 'fadeOut',
        });
        $('.sponsors').owlCarousel({
            loop: true,
            margin: 80,
            smartSpeed: 1000,
            responsiveClass: true,
            nav: true,
            dots: false,
            autoplay: true,
            responsive: {
                0: {
                    items: 1,
                    nav: false,
                    dots: false
                },
                600: {
                    items: 3,
                    nav: false
                },
                1000: {
                    items: 5,
                    nav: false,
                    dots: false
                }
            }
        });
        $('.team-carouzel').owlCarousel({
            loop: true,
            margin: 28,
            smartSpeed: 1000,
            responsiveClass: true,
            nav: true,
            dots: false,
            responsive: {
                0: {
                    items: 1,
                    nav: false,
                    dots: false
                },
                600: {
                    items: 2,
                    nav: true
                },
                1000: {
                    items: 3,
                    nav: true,
                }
            }
        });
        $('.live-streamer').owlCarousel({
            items: 8,
            loop: true,
            margin: 20,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 3,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 8,
                }
            }
        });
        $('.video-clips').owlCarousel({
            items: 3,
            loop: true,
            margin: 20,
            autoplay: false,
            autoplayTimeout: 1500,
            smartSpeed: 1000,
            autoplayHoverPause: true,
            nav: true,
            dots: false,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 3,
                },
                600: {
                    items: 3,
                },
                1000: {
                    items: 3,
                }
            }
        });
    }
    if ($.isFunction($.fn.slick)) {
        $('.slick-single').slick();
        $('.slick-multiple').slick({
            infinite: true,
            slidesToShow: 4,
            slidesToScroll: 4
        });
        $('.slick-autoplay').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 2000,
        });
        $('.pod-caro1').slick({
            slidesToShow: 6,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 0,
            arrows: false,
            speed: 13000,
            pauseOnHover: false,
            cssEase: 'linear',
            infinite: true,
        });
        $('.pod-caro2').slick({
            slidesToShow: 6,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 0,
            arrows: false,
            speed: 11000,
            pauseOnHover: false,
            cssEase: 'linear',
            infinite: true,
        });
        $('.pod-caro3').slick({
            slidesToShow: 6,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 0,
            arrows: false,
            speed: 15000,
            pauseOnHover: false,
            cssEase: 'linear',
            infinite: true,
        });
        $('.slick-center-mode').slick({
            centerMode: true,
            centerPadding: '60px',
            slidesToShow: 3,
            responsive: [{
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 3
                }
            }, {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1
                }
            }]
        });
        $('.slick-fade-effect').slick({
            dots: true,
            infinite: true,
            speed: 500,
            fade: true,
            cssEase: 'linear'
        });
        $('.slider-for').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            fade: true,
            asNavFor: '.slider-nav'
        });
        $('.slider-nav').slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            asNavFor: '.slider-for',
            centerMode: true,
            focusOnSelect: true
        });
        $('.slider-for-gold').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            slide: 'li',
            fade: false,
            asNavFor: '.slider-nav-gold'
        });
        $('.slider-nav-gold').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            asNavFor: '.slider-for-gold',
            dots: false,
            arrows: false,
            slide: 'li',
            vertical: true,
            centerMode: true,
            centerPadding: '0',
            focusOnSelect: true,
            responsive: [{
                breakpoint: 768,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    infinite: true,
                    vertical: true,
                    centerMode: true,
                    dots: false,
                    arrows: false
                }
            }, {
                breakpoint: 641,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                    infinite: true,
                    vertical: true,
                    centerMode: true,
                    dots: false,
                    arrows: false
                }
            }]
        });
    }
    if ($.isFunction($.fn.jalendar)) {
        $('#yourId').jalendar({
            customDay: '11/01/2024',
            color: '#577e9a',
            color2: '#57c8bf',
            lang: 'EN',
            sundayStart: true
        });
    }
    if ($.isFunction($.fn.mmenu)) {
        $(function () {
            $('#menu').mmenu();
            $('#shoppingbag').mmenu({
                navbar: {
                    title: 'General Setting'
                },
                offCanvas: {
                    position: 'right'
                }
            });
            $('.mh-head.first').mhead({
                scroll: {
                    hide: 200
                }
            });
            $('.mh-head.second').mhead({
                scroll: false
            });
        });
    }
    $("span.main-menu").on("click", function () {
        $(".side-panel").toggleClass('active');
        $(".theme-layout").toggleClass('active');
        return false;
    });
    $('.theme-layout').on("click", function () {
        $(this).removeClass('active');
        $(".side-panel").removeClass('active');
    });
    $('button.signup').on("click", function () {
        $('.login-reg-bg').addClass('show');
        return false;
    });
    $('.already-have').on("click", function () {
        $('.login-reg-bg').removeClass('show');
        return false;
    });
    if ($.isFunction($.fn.downCount)) {
        $('.countdown').downCount({
            date: '11/12/2026 12:00:00',
            offset: +10
        });
    }
    if ($.isFunction($.fn.counterUp)) {
        $('.counter').counterUp({
            delay: 10,
            time: 1000
        });
    }
    jQuery(".post-comt-box textarea").on("keydown", function (event) {
        if (event.keyCode == 13) {
            var comment = jQuery(this).val();
            var parent = jQuery(".showmore").parent("li");
            var comment_HTML = '<li><div class="comet-avatar"><img alt="" src="images/resources/comet-2.jpg"></div><div class="we-comment"><h5><a title="" href="time-line.html">Sophia</a></h5><p>' + comment + '</p><div class="inline-itms"><span>1 minut ago</span><a title="Reply" href="#" class="we-reply"><i class="fa fa-reply"></i></a><a title="" href="#"><i class="fa fa-heart"></i></a></div></div></li>';
            $(comment_HTML).insertBefore(parent);
            jQuery(this).val('');
        }
    });
    $('.message-list > li > span.star-this').on("click", function () {
        $(this).toggleClass('starred');
    });
    $('.message-list > li > span.make-important').on("click", function () {
        $(this).toggleClass('important-done');
    });
    $('#select_all').on("click", function (event) {
        if (this.checked) {
            $('input:checkbox.select-message').each(function () {
                this.checked = true;
            });
        } else {
            $('input:checkbox.select-message').each(function () {
                this.checked = false;
            });
        }
    });
    $(".delete-email").on("click", function () {
        $(".message-list .select-message").each(function () {
            if (this.checked) {
                $(this).parent().slideUp();
            }
        });
    });
    $('li.menu-item-has-children > a').on('click', function () {
        $(this).parent().siblings().children('ul').slideUp();
        $(this).parent().siblings().removeClass('active');
        $(this).parent().children('ul').slideToggle();
        $(this).parent().toggleClass('active');
        return false;
    });
    $(function () {
        $("#price-range").slider({
            range: "max",
            min: 18,
            max: 65,
            value: 18,
            step: 1,
            slide: function (event, ui) {
                $("#priceRange").val(ui.value + " Years");
            }
        });
        $("#priceRange").val($("#price-range").slider("value") + " Years");
    });
    $(function () {
        $("#slider-range").slider({
            range: true,
            min: 0,
            max: 500,
            values: [75, 300],
            slide: function (event, ui) {
                $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
            }
        });
        $("#amount").val("$" + $("#slider-range").slider("values", 0) + " - $" + $("#slider-range").slider("values", 1));
    });
    var skills = {
        ht: 78,
        jq: 60,
        sk: 45,
        ph: 25,
        il: 15,
        in: 5
    };
    $.each(skills, function (key, value) {
        var skillbar = $('.' + key);
        skillbar.animate({
            width: value + "%"
        }, 3000, function () {
            $(".speech-bubble").fadeIn();
        });
    });
});
(function () {
    window.onload = function () {
        var totalProgress, progres;
        const circles = document.querySelectorAll('.progres');
        for (var i = 0; i < circles.length; i++) {
            totalProgress = circles[i].querySelector('circle').getAttribute('stroke-dasharray');
            progress = circles[i].parentElement.getAttribute('data-percent');
            circles[i].querySelector('.bar').style['stroke-dashoffset'] = totalProgress * progress / 100;
        }
    }
        ;
}
)();
