function setFormDatasourcePath(alpacaFormJson, formsUrl) {
    for (var property in alpacaFormJson.options.fields) {
        if (alpacaFormJson.options.fields.hasOwnProperty(property)) {
            var theProp = alpacaFormJson.options.fields[property];
            if (typeof (theProp.dataSource) == "string") {
                theProp.dataSource = formsUrl + theProp.dataSource;
            }
        }
    }
}

function setFormActionPath(alpacaFormJson, formsUrl) {
    var action = alpacaFormJson.options.form.attributes.action;
    if (typeof (action) == "string") {
        alpacaFormJson.options.form.attributes.action = formsUrl + action;
    }
}