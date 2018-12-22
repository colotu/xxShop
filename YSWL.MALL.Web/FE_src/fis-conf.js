// default settings. fis3 release

fis.set('project.ignore', ['node_modules/**', 'output/**', '.git/**', 'fis-conf.js', 'package.json', 'js/loader.js']); // 排除某些文件, 'lib/**/*'
fis.match('::image', {
  useHash: true
});

//fis-conf.js
// 启用插件
fis.hook('relative');
// Global end

// default media is `dev`
fis.media('dev').match('*', {
    useHash: false,
    optimizer: null
});

//上线时压缩
fis.media('prod').match('**', {// 让所有文件，都使用相对路径。
    relative: true
}).match('*.css', {
    optimizer: fis.plugin('clean-css'), // css 压缩
    rExt: '.min.css',//后缀
    useHash: false
}).match('*.js', {
    optimizer: fis.plugin('uglify-js'),//js压缩
    rExt: '.min.js',//后缀
    useHash: false,
}).match('*.png', {
    optimizer: fis.plugin('png-compressor'),// png 图片压缩
    useHash: false
})
// extends GLOBAL config
//fis.media('production');