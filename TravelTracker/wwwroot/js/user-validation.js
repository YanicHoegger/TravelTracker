$(function () {
        jQuery.validator.addMethod('email',
            function (value, element, params) {
                var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return regex.test(value);
            });

        jQuery.validator.unobtrusive.adapters.add('email', [ ],
            function (options) {
                options.rules['email'] = {};
                options.messages['email'] = options.message;
            });

        jQuery.validator.addMethod('require-digit',
            function (value, element, params) {
                if(params[0]) {
                    return /\d/.test(value);
                }
                return true;
            });

        jQuery.validator.unobtrusive.adapters.add('require-digit', [ 'is-required' ],
            function (options) {
                options.rules['require-digit'] = [options.message];
                options.messages['require-digit'] = 'Password requires a digit';
            });

        jQuery.validator.addMethod('passwordlength',
            function (value, element, params) {
                return value.length >= params[0] ;
            });

        jQuery.validator.unobtrusive.adapters.add('passwordlength', [ 'length' ],
            function (options) {
                options.rules['passwordlength'] = [parseInt(options.params['length'])];
                options.messages['passwordlength'] = options.message;
            });

        jQuery.validator.addMethod('uppercase',
            function (value, element, params) {
                return /[A-Z]/.test(value) ;
            });

        jQuery.validator.unobtrusive.adapters.add('uppercase', [ ],
            function (options) {
                var length = parseInt(options.message);
                options.rules['uppercase'] = { };
                options.messages['uppercase'] = options.message;
            });

        //TODO: Validate that password and repassword are same      
    }(jQuery));