'use strict';
app.filter('utcToLocal', FilterUtcDate)
    .filter('phoneNumber', FilterPhoneNumber)
    .constant('ngAppSettings', {
        serviceBase: '',
        clientId: 'ngAuthApp',
        facebookAppId: '464285300363325',
        request: {
            pageSize: '10',
            pageIndex: 0,
            status: '2',
            orderBy: 'Priority',
            direction: '0',
            fromDate: null,
            toDate: null,
            keyword: '',
            key:'',
            query:''
        },
        privacies:  [
            'VND',
            'USD'
        ],
        pageSizes: [
            '5',
            '10',
            '15',
            '20'
        ],
        directions: [
            {
                value: '0',
                title: 'Asc'
            },
            {
                value: '1',
                title: 'Desc'
            }
        ],
        orders: [
            {
                value: 'CreatedDateTime',
                title: 'Created Date'
            }
            ,
            {
                value: 'Priority',
                title: 'Priority'
            },

            {
                value: 'Title',
                title: 'Title'
            }
        ],
        contentStatuses: [
            'Deleted',
            'Preview',
            'Published',
            'Draft',
            'Schedule'
        ],        
        editorConfigurations: {
            core: {},
            plugins: {
                btnsDef: {
                    // Customizables dropdowns
                    image: {
                        dropdown: ['insertImage', 'upload', 'base64', 'noembed'],
                        ico: 'insertImage'
                    }
                },
                btns: [                    
                    ['undo', 'redo'],
                    ['table'],
                    ['emoji'],
                    ['formatting'],
                    ['strong', 'em', 'del', 'underline'],
                    ['link'],
                    ['image'],
                    ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
                    ['unorderedList', 'orderedList'],
                    ['foreColor', 'backColor'],
                    ['preformatted'],
                    ['horizontalRule'],
                    ['fullscreen'],
                    ['viewHTML']
                ],
                plugins: {
                    // Add imagur parameters to upload plugin
                    upload: {
                        serverPath: 'https://api.imgur.com/3/image',
                        fileFieldName: 'image',
                        headers: {
                            'Authorization': 'Client-ID 9e57cb1c4791cea'
                        },
                        urlPropertyName: 'data.link'
                    },
                    table:{},
                    fontfamily: {
                        init: function (trumbowyg) {
                            trumbowyg.o.plugins.fontfamily = trumbowyg.o.plugins.fontfamily || defaultOptions;
                            trumbowyg.addBtnDef('fontfamily', {
                                dropdown: buildDropdown(trumbowyg),
                                hasIcon: false,
                                text: trumbowyg.lang.fontFamily
                            });
                        }
                    }
                }
            }            
        },
        dataTypes: [
            { title: 'Custom', value: 0 },
            { title: 'DateTime', value: 1 },
            { title: 'Date', value: 2 },
            { title: 'Time', value: 3 },
            { title: 'Duration', value: 4 },
            { title: 'PhoneNumber', value: 5 },
            { title: 'Currency', value: 6 },
            { title: 'Text', value: 7 },
            { title: 'Html', value: 8 },
            { title: 'MultilineText', value: 9 },
            { title: 'EmailAddress', value: 10 },
            { title: 'Password', value: 11 },
            { title: 'Url', value: 12 },
            { title: 'ImageUrl', value: 13 },
            { title: 'CreditCard', value: 14 },
            { title: 'PostalCode', value: 15 },
            { title: 'Upload', value: 16 },

        ]
    });

function FilterUtcDate($filter) {
    return function (utcDateString, format) {
        format = format || 'MM.dd.yyyy - hh:mm:ss a';
        // return if input date is null or undefined
        if (!utcDateString) {
            return;
        }

        // append 'Z' to the date string to indicate UTC time if the timezone isn't already specified
        if (utcDateString.indexOf('Z') === -1 && utcDateString.indexOf('+') === -1) {
            utcDateString += 'Z';
        }

        // convert and format date using the built in angularjs date filter
        return $filter('date')(utcDateString, format);
    };
}
function buildDropdown(trumbowyg) {
    var dropdown = [];

    $.each(trumbowyg.o.plugins.fontfamily.fontList, function (index, font) {
        trumbowyg.addBtnDef('fontfamily_' + index, {
            title: '<span style="font-family: ' + font.family + ';">' + font.name + '</span>',
            hasIcon: false,
            fn: function () {
                trumbowyg.execCmd('fontName', font.family, true);
            }
        });
        dropdown.push('fontfamily_' + index);
    });

    return dropdown;
}
function FilterPhoneNumber(){
    return function (phone) {
        return phone.replace(/^([0-9]{3})([0-9]{3})([0-9]{4,})$/, '($1) $2-$3');
    };
}

function FilterMoney() {
    return function (money) {
        return money.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    };
}