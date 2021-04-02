function unix_now() {
    return Math.floor(Date.now()); //miliseconds
}

function sleep(t) {
    return new Promise(resolve => setTimeout(resolve, t));
}

async function submitForm(path, fields, timeout = 3000) {
    //console.log("Submit!!! : " + unix_now_seconds());
    var data = new Object;
    for (const key in fields) {
        if (fields.hasOwnProperty(key)) {
            data[key] = fields[key];
        }
    }

    var rcv = false;
    var request_time = unix_now();
    var limit_time = request_time + timeout;
    var ret = null;
    try {
        $.ajax({
            url: path,
            type: 'POST',
            data: data,
            success: function (result) {
                ret = result;
                rcv = true;
            },
            async: false
        })

    }
    catch (err) {
        rcv = true;
    }

    while (rcv == false && unix_now() < limit_time) {
        await sleep(100);
    }

    return ret;
}




export {
    submitForm
};