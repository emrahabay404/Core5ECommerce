

var basari = function (response) {
   swal("İşlem Başarıyla gerçekleşti", "", "success");
   $('#urneklemdl').modal('hide');
   $('#modaldene').modal('hide');
   $('#modal1').modal('hide');
   $('#modal2').modal('hide');

};

var hata = function (response) {
   swal("Hata oluştu. İşlem başarısız", "");
}


$(function () {
   var placeholderelement = $('#Placeholderelement');
   $('button[data-toggle="ajax-modal"]').click(function (event) {
      var url = $(this).data('url');
      var decodeurl = decodeURIComponent(url);
      $.get(decodeurl).done(function (data) {
         placeholderelement.html(data);
         placeholderelement.find('#ordermodal').modal('show');
      })
   });
});




//placeholderelement.on('click', '[data-save="modal"]', function (event) {
//    event.preventDefault();
//    var form = $(this).parents('urneditmdl').find('form');
//    var actionurl = form.attr('action');
//    var sendData = form.serialize();
//    $.post(actionurl, sendData).done(function (data) {
//        placeholderelement.find('urneditmdl').modal('hide');
//    });
//});





      //Klavyeye her basıldıgında filtreleme yapmak için.
                                                                                        //$("#urunara").on("keyup", function () {
                                                                                        //    var txtenter = $(this).val();
                                                                                        //    $("table tr").each(function (results) {
                                                                                        //        if (results !== 0) {
                                                                                        //            var id = $(this).find("td:nth-child(3)").text();
                                                                                        //            if (id.indexOf(txtenter) !== 0 && id.toLowerCase().indexOf(txtenter.toLowerCase()) < 0) {
                                                                                        //                $(this).hide();
                                                                                        //            }
                                                                                        //            else {
                                                                                        //                $(this).show();
                                                                                        //            }
                                                                                        //        }
                                                                                        //    });
                                                                                        //});