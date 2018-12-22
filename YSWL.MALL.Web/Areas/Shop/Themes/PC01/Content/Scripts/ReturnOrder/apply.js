$(function () {
    $('.returnCount').OnlyNum();

    $('[name="returngoodstype"]').click(function(){
        var _val=parseInt($(this).val());
        if(_val==1 ){
            $('#table_Items').hide();
        }else{
            $('#table_Items').show();
        }
    });

      //上传图片
     qqupload();
    //改变上传图片按钮的样式
     $('.qq-upload-button').css({'backgroundColor':'#A59D89','width':'64','padding':'0'});

    
    $('#btnSubmit').click(function () {
        //获取要退货的商品 (不包含赠品,赠品在后台获取)
        var oid = $('#hidoid').val();
        if (isNaN(oid)) {
            //ShowFailTip("");
            return;
        }

        //退货类型
        var returngoodstype = parseInt($('[name="returngoodstype"]:checked').val());
        if (isNaN(returngoodstype) || returngoodstype <= 0) {
            ShowFailTip("请选择退货类型");
            return;
        }
         //如果只有赠品（没有正常销售的商品）就把退货类型改为整单退  
        if($('[id^="checkboxprod_"][prtype=1]').length<=0){
            returngoodstype=1;
        }
        var items = $('[id^="checkboxprod_"][prtype=1]:checked');
        var json = [];
        if(returngoodstype!=1){ //1是 整单退 在后台获取
             if (items.length <= 0) {
                 ShowFailTip("请选择商品");
                 return;
             }
      
             var itemId = 0;
             var count = 0;
             for (var i = 0; i < items.length; i++) {
                    itemId = parseInt(items.eq(i).attr('itemId'));
                    count = parseInt($('#textcount_' + itemId).val());
                    if (isNaN(count) || count <= 0) {
                          ShowFailTip("退货数量必须大于0");
                          return;
                     }
                     json.push({ "itemId": itemId, "count": count });
               }
         }

      
       
     
        var serviceType = parseInt($('[name="serviceType"]:checked').val());
        if (isNaN(serviceType) || serviceType <= 0) {
            ShowFailTip("请选择服务类型");
            return;
        }
        var content = $.trim($('#apply_Content').val());
        if (content.length <= 0) {
            ShowFailTip("请填写退货原因");
            return;
        }
        var pickWareType = parseInt($('[name="pickWareType"]:checked').val());
        if (isNaN(pickWareType) || pickWareType < 0) {
            ShowFailTip("请选择返回方式");
            return;
        }

        var regionId = parseInt($('#hfSelectedNode').val());
        if (isNaN(regionId) || regionId <= 0) {
            ShowFailTip("请选择地区");
            return;
        }

        var address = $.trim($('#pick_Address').val());
        if (address.length <= 0) {
            ShowFailTip("请填写地址");
            return;
        }

        var name = $.trim($('#applyUserName').val());
        if (name.length <= 0) {
            ShowFailTip("请填写联系人姓名");
            return;
        }

        var phone = $.trim($('#applyPhone').val());
        if (phone.length <= 0) {
            ShowFailTip("请填写联系人电话");
            return;
        }
        var regs = /^1([38][0-9]|4[57]|5[^4])\d{8}$/;
        if (!regs.test(phone)) {
            ShowFailTip("请填写有效的手机号码！");
            return;
        }

        var imagesurlPath = $('[name="UploadPhotoPath"]').val();
        var imagesurlName = $('[name="UploadPhotoNames"]').val();


        $.ajax({
            type: "POST",
            dataType: "text",
            timeout: 0,
            url: $YSWL.BasePath + "ReturnOrder/AjaxRetuApply",
            data: { oId: oid,RGoodsType:returngoodstype,Items: JSON.stringify(json), ServiceType: serviceType, Content: content, PickType: pickWareType, RegionId: regionId, Address: address, Name: name, Phone: phone,ImagesurlPath:imagesurlPath, ImagesurlName:imagesurlName},
            success: function (data) {
                switch (data) {
                    case "True":
                        ShowSuccessTip("提交成功");
                        window.location.replace($YSWL.BasePath + "ReturnOrder/Return"); 
                        //跳转
                        break;
                    case "Fail":
                        ShowFailTip("提交失败");
                        break;
                    case "SELECTALLITEMS":
                        ShowFailTip("若要将全部商品退回,请使用【整单退】");
                        break;
                    case "IsNotMeetCondition":
                        //不满足退单条件 
                        break;
                    case "Illegal":
                        //数据重复
                        break;
                    case "COUNTISTOOBIG":
                        //数量超过购买数量
                        break;
                    case "ITEMSISNULL": ////项为空
                        break;
                    case "AMOUNTISNULL":
                        //总金额小于=0
                        break;
                    case "False":
                        //数据不完整
                        break;
                    default:
                        break;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowFailTip("操作失败：" + errorThrown);
            }
        });
    });


    var self_val = 0;
    $('.returnCount').focus(function () {
        self_val = $(this).val();
    }).blur(function () {
        var count = $(this).attr('count');
        var _val = $(this).val();
        if (count < _val) {
            ShowFailTip("退货数量不能大于购买数量！");
            $(this).val(self_val);
            return false;
        }
    });

});

//上传图片按钮
    var qqupload = function() {
        var ulbtnparent = $("[name='UploadPhoto']").parent();
        new qq.FineUploader({
            element: $("[name='UploadPhoto']")[0],
            request: {
                endpoint: '/UploadMultipleFileHandler.aspx'
            },
            text: {
                uploadButton: '上传图片'
            },
            multiple: true,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
                itemLimit: 5,
                sizeLimit: 5242880, // 50 kB = 50 * 1024 bytes
            },
            messages: {
                tooManyItemsError: "您已上传{itemLimit}张，最多上传 {itemLimit}张图片.",//{netItems}
                sizeError: "{file}文件过大, 文件最大为 {sizeLimit}.",
            },
            callbacks: {
                onComplete: function(id, fileName, responseJSON) {
                    $(".qq-upload-list").hide();
                    if (responseJSON.success) {
                         ulbtnparent.append(('<div style="display:inline-block;line-height: 45px;"><img src="{0}"  width="45px" height="45px"/><span  onclick="nameDel(this);"  item="{0}" itemname="{1}"  >删除</span></div>').format(
                            responseJSON.path.format(responseJSON.names), responseJSON.names));
                        ShowSuccessTip('上传成功！');
                        ulbtnparent.find('[name="UploadPhotoPath"]').val(ulbtnparent.find('[name="UploadPhotoPath"]').val() + '|' + responseJSON.path.format(responseJSON.names));
                        ulbtnparent.find('[name="UploadPhotoNames"]').val(ulbtnparent.find('[name="UploadPhotoNames"]').val() + '|' + responseJSON.names);
                        imghover();

                    } else {
                        ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试：");
                    }
                }
            }
        });
    };


      //鼠标移入移出图片
    var imghover = function() {
       $('.reviewimg-upload').find('img').parent('div').unbind('hover').hover(function () {  
          $(this).find('span').css('display','inline-block');
       }, function () {
          $(this).find('span').css('display','none');
       });
    };

     //删除图片
      function nameDel(sender) {
        var ulbtnparent=  $(sender).parents('.reviewimg-upload');
        var targetVal =  $(sender).attr('item');
        $(sender).parent('div').remove();
        var pathArray =ulbtnparent.find('[name="UploadPhotoPath"]').val().split('|');
        var index = pathArray.getIndexByValue(targetVal);
        pathArray.remove(index);
        ulbtnparent.find('[name="UploadPhotoPath"]').val(pathArray.join('|'));

        var nameVal = $(sender).attr('itemname');
        var nameArray = ulbtnparent.find('[name="UploadPhotoNames"]').val().split('|');
        var indexname = nameArray.getIndexByValue(nameVal);
        nameArray.remove(indexname);
        ulbtnparent.find('[name="UploadPhotoNames"]').val(nameArray.join('|'));
    }
