module.exports = function (config) {
    var libraries = [
    'www/source/js/lib/jquery-3.3.1.min.js',
    'www/source/js/lib/bootstrap.min.js',
    'www/source/js/lib/jquery.validate.min.js',
    'www/source/js/lib/jquery.validate.unobtrusive.min.js',
    'www/source/js/lib/pikaday.js'
    ];

    var commonFiles = [
        'www/source/js/site.js',
        'www/source/js/layout.js',
        'www/source/js/recheckin.js'
    ]

    var fileToInclude = [
        ...libraries, ...commonFiles,
        'www/source/js/components/*.js',
        'www/source/js/spec/**/*.spec.js'
    ]

    config.set({
        port: 9975,
        frameworks: ['jasmine'],
        files: fileToInclude,
        browsers: ['PhantomJS'],
        reporters: ['progress', 'coverage'],
        preprocessors: {
            'www/source/js/components/*.js': ['coverage'],
            'www/source/js/pages/**/*.js': ['coverage'],
            'www/source/js/*.js': ['coverage']
        },
        coverageReporter: {
            type: 'html',
            dir: 'coverage/'
        }
    });
};