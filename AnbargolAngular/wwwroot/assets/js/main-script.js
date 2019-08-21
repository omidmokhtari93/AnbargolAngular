(function ($) {
  $(document).ready(function () {
    $('#cssmenu > ul > li > a').click(function () {
      $('#cssmenu li').removeClass('active');
      $(this).closest('li').addClass('active');
      var checkElement = $(this).next();
      if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
        $(this).closest('li').removeClass('active');
        checkElement.slideUp('normal');
      }
      if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
        $('#cssmenu ul ul:visible').slideUp('normal');
        checkElement.slideDown('normal');
      }
      if ($(this).closest('li').find('ul').children().length == 0) {
        return true;
      } else {
        return false;
      }
    });
  });
})(jQuery);

var customOptions = {
  placeholder: "روز / ماه / سال",
  twodigit: true,
  closeAfterSelect: true,
  nextButtonIcon: "fa fa-arrow-circle-right",
  previousButtonIcon: "fa fa-arrow-circle-left",
  buttonsColor: "blue",
  forceFarsiDigits: true,
  markToday: true,
  markHolidays: true,
  highlightSelectedDay: true,
  sync: true,
  gotoToday: true
};
