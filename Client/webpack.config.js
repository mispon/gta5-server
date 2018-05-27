'use strict';

const glob = require('glob')
const webpack = require('webpack')
const path = require("path")
let NODE_ENV = 'production'
if (process.env.NODE_ENV) {
    NODE_ENV = process.env.NODE_ENV.replace(/^\s+|\s+$/g, "")
}

let files = glob.sync('./src/**/*.ts')
console.log('Загружено файлов: ' + files.length)

module.exports = {
    entry: {
        main: files
    },
    output: {
        path: __dirname + "/public",
        filename: "build.js"
    },
    watch: NODE_ENV == 'dev',
    module: {
        loaders: [
          {
            test: /\.ts/,
            loader: 'awesome-typescript-loader?configFileName=tsconfig.json'
          }
        ]
    },
    resolve: {
        extensions: ['.ts']
    },
    //devtool: NODE_ENV == 'dev' ? 'eval' : null,
    plugins: [
        new webpack.EnvironmentPlugin('NODE_ENV'),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            swal: "sweetalert2"
        }),
         new webpack.optimize.UglifyJsPlugin({
            compress: { warnings: false }
        })
    ]
};
