window._wpemojiSettings = { "baseUrl": "https:\/\/s.w.org\/images\/core\/emoji\/12.0.0-1\/72x72\/", "ext": ".png", "svgUrl": "https:\/\/s.w.org\/images\/core\/emoji\/12.0.0-1\/svg\/", "svgExt": ".svg", "source": { "concatemoji": "https:\/\/era.sa\/wp-includes\/js\/wp-emoji-release.min.js?ver=5.4.1" } };
/*! This file is auto-generated */
!function (e, a, t) { var r, n, o, i, p = a.createElement("canvas"), s = p.getContext && p.getContext("2d"); function c(e, t) { var a = String.fromCharCode; s.clearRect(0, 0, p.width, p.height), s.fillText(a.apply(this, e), 0, 0); var r = p.toDataURL(); return s.clearRect(0, 0, p.width, p.height), s.fillText(a.apply(this, t), 0, 0), r === p.toDataURL() } function l(e) { if (!s || !s.fillText) return !1; switch (s.textBaseline = "top", s.font = "600 32px Arial", e) { case "flag": return !c([127987, 65039, 8205, 9895, 65039], [127987, 65039, 8203, 9895, 65039]) && (!c([55356, 56826, 55356, 56819], [55356, 56826, 8203, 55356, 56819]) && !c([55356, 57332, 56128, 56423, 56128, 56418, 56128, 56421, 56128, 56430, 56128, 56423, 56128, 56447], [55356, 57332, 8203, 56128, 56423, 8203, 56128, 56418, 8203, 56128, 56421, 8203, 56128, 56430, 8203, 56128, 56423, 8203, 56128, 56447])); case "emoji": return !c([55357, 56424, 55356, 57342, 8205, 55358, 56605, 8205, 55357, 56424, 55356, 57340], [55357, 56424, 55356, 57342, 8203, 55358, 56605, 8203, 55357, 56424, 55356, 57340]) }return !1 } function d(e) { var t = a.createElement("script"); t.src = e, t.defer = t.type = "text/javascript", a.getElementsByTagName("head")[0].appendChild(t) } for (i = Array("flag", "emoji"), t.supports = { everything: !0, everythingExceptFlag: !0 }, o = 0; o < i.length; o++)t.supports[i[o]] = l(i[o]), t.supports.everything = t.supports.everything && t.supports[i[o]], "flag" !== i[o] && (t.supports.everythingExceptFlag = t.supports.everythingExceptFlag && t.supports[i[o]]); t.supports.everythingExceptFlag = t.supports.everythingExceptFlag && !t.supports.flag, t.DOMReady = !1, t.readyCallback = function () { t.DOMReady = !0 }, t.supports.everything || (n = function () { t.readyCallback() }, a.addEventListener ? (a.addEventListener("DOMContentLoaded", n, !1), e.addEventListener("load", n, !1)) : (e.attachEvent("onload", n), a.attachEvent("onreadystatechange", function () { "complete" === a.readyState && t.readyCallback() })), (r = t.source || {}).concatemoji ? d(r.concatemoji) : r.wpemoji && r.twemoji && (d(r.twemoji), d(r.wpemoji))) }(window, document, window._wpemojiSettings);

_stq = window._stq || [];
_stq.push(['view', { v: 'ext', j: '1:8.6.1', blog: '168353511', post: '404', tz: '0', srv: 'era.sa' }]);
_stq.push(['clickTrackerInit', '168353511', '404']);00

document.documentElement.classList.add(
    'jetpack-lazy-images-js-enabled'
);



/* <![CDATA[ */
var VPData = { "__": { "couldnt_retrieve_vp": "Couldn&#039;t retrieve Visual Portfolio ID.", "pswp_close": "Close (Esc)", "pswp_share": "Share", "pswp_fs": "Toggle fullscreen", "pswp_zoom": "Zoom in\/out", "pswp_prev": "Previous (arrow left)", "pswp_next": "Next (arrow right)", "pswp_share_fb": "Share on Facebook", "pswp_share_tw": "Tweet", "pswp_share_pin": "Pin it", "fancybox_close": "Close", "fancybox_next": "Next", "fancybox_prev": "Previous", "fancybox_error": "The requested content cannot be loaded. <br \/> Please try again later.", "fancybox_play_start": "Start slideshow", "fancybox_play_stop": "Pause slideshow", "fancybox_full_screen": "Full screen", "fancybox_thumbs": "Thumbnails", "fancybox_download": "Download", "fancybox_share": "Share", "fancybox_zoom": "Zoom" }, "settingsPopupGallery": { "vendor": "photoswipe", "show_arrows": true, "show_counter": true, "show_zoom_button": true, "show_fullscreen_button": true, "show_share_button": true, "show_close_button": true, "show_download_button": false, "show_slideshow": false, "show_thumbs": true }, "screenSizes": [320, 576, 768, 992, 1200] };
        /* ]]> */



jQuery(document).ready(function () {

    let scrollTop = 0;
    let easedScrollTop = 0;
    let $content = document.querySelector('.js-content');
    let $fakeScroll = document.querySelector('.js-fake-scroll');

    document.addEventListener('scroll', () => {
        scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        
    });

    function resize() {
        $fakeScroll.style.height = $content.clientHeight + 'px';
    }

    window.addEventListener('resize', resize);

    function update() {
        requestAnimationFrame(update);
        easedScrollTop += (scrollTop - easedScrollTop) * 0.1;
        $content.style.transform = 'translateY(' + (easedScrollTop * -1) + 'px) translateZ(0)';
    }

    resize();
    update();

});

jQuery(document).ready(function () {

    let scrollTop = 0
    let easedScrollTop = 0
    let $content = document.querySelector('.js-content')
    let $fakeScroll = document.querySelector('.js-fake-scroll')

    document.addEventListener('scroll', () => {
        scrollTop = window.pageYOffset || document.documentElement.scrollTop
        
    })

    function resize() {
        $fakeScroll.style.height = $content.clientHeight + 'px'
    }

    window.addEventListener('resize', resize)

    function update() {
        requestAnimationFrame(update)
        easedScrollTop += (scrollTop - easedScrollTop) * 0.1
        $content.style.transform = 'translateY(' + (easedScrollTop * -1) + 'px) translateZ(0)'
    }

    resize()
    update()

});
