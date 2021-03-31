function submitForm(path, fields) {
    console.log("Submit!!!");
    var data = new Object;
    for (const key in fields) {
        if (fields.hasOwnProperty(key)) {
            data[key] = fields[key];
            //const hiddenField = document.createElement('input');
            //hiddenField.type = 'hidden';
            //hiddenField.name = key;
            //hiddenField.value = fields[key];
            //form.appendChild(hiddenField);
        }
    }

    var ret = null;
    $.ajax({
        url: path,
        type: 'POST',
        data: data,
        success: function (result) {
            ret = result;
        },
        async: false
    })
    return ret;

    //const form = document.createElement('form');
    //form.method = 'post';
    //form.action = path;

    //for (const key in fields) {
    //    if (fields.hasOwnProperty(key)) {
    //        const hiddenField = document.createElement('input');
    //        hiddenField.type = 'hidden';
    //        hiddenField.name = key;
    //        hiddenField.value = fields[key];
    //        form.appendChild(hiddenField);
    //    }
    //}

    //document.body.appendChild(form);
    //form.submit(function () {
    //    console.log(this);
    //});
}
export {
    submitForm
};