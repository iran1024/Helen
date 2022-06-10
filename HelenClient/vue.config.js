module.exports = {
  publicPath: process.env.NODE_ENV === 'production' ? '/helen' : '/',
  outputDir: 'helen',
  assetsDir: 'assets',
  transpileDependencies: true,
  runtimeCompiler: true,
  chainWebpack: config => {
    config
      .plugin('html')
      .tap(args => {
        args[0].title = 'Helen System'
        return args
      })
  }
}
