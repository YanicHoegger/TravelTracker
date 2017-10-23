$(function () {
        jQuery.validator.addMethod('digit',
            function (value, element, params) {
                return /\d/.test(value);
            });

        jQuery.validator.unobtrusive.adapters.add('digit', [ ],
            function (options) {
                options.rules['digit'] = { };
                options.messages['digit'] = options.message;
            });

        jQuery.validator.addMethod('passwordlength',
            function (value, element, params) {
                return value.length >= params[0];
            });

        jQuery.validator.unobtrusive.adapters.add('passwordlength', [ 'length' ],
            function (options) {
                options.rules['passwordlength'] = [parseInt(options.params['length'])];
                options.messages['passwordlength'] = options.message;
            });

        jQuery.validator.addMethod('nonalphanumeric',
            function (value, element, params) {
                return /[\W_]/.test(value);
            });

        jQuery.validator.unobtrusive.adapters.add('nonalphanumeric', [ ],
            function (options) {
                options.rules['nonalphanumeric'] = { };
                options.messages['nonalphanumeric'] = options.message;
            });

        jQuery.validator.addMethod('uppercase',
            function (value, element, params) {
                return /[A-Z]/.test(value);
            });

        jQuery.validator.unobtrusive.adapters.add('uppercase', [ ],
            function (options) {
                options.rules['uppercase'] = { };
                options.messages['uppercase'] = options.message;
            });

        jQuery.validator.addMethod('lowercase',
            function (value, element, params) {
                return /[a-z]/.test(value);
            });

        jQuery.validator.unobtrusive.adapters.add('lowercase', [ ],
            function (options) {
                options.rules['lowercase'] = { };
                options.messages['lowercase'] = options.message;
            });

        //TODO: Validate that password and repassword are same      
    }(jQuery));