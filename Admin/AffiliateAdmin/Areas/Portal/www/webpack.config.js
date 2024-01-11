const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const merge = require("webpack-merge");
const CleanWebpackPlugin = require("clean-webpack-plugin");

const { environment, projectPath } = require("./config-utils/constant");
const entry = require("./config-utils/entry");
const layoutEntry = require("./config-utils/layoutEntry");

let pathsToClean = ["dist/js/**/*.*", "dist/style/**/*.*"];

let cleanOptions = {
    root: path.resolve(__dirname, "."),
    verbose: true,
    dry: false
};

const relativeOutputPath = projectPath.relativeOutputPath;

module.exports = env => {
    env = environment[env] || environment.development;
    const isDevBuild = env !== environment.production;

    const sharedConfig = () => ({
        mode: env,
        stats: { modules: false },
        resolve: { extensions: [".js", ".jsx"] },
        output: {
            path: projectPath.rootPath,
            filename: "[name].js",
            chunkFilename: relativeOutputPath + '/js/dynamic-chunks/[name]-[chunkhash].chunk.js'
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                use: "babel-loader",
                exclude: /(node_modules|bower_components)/
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf)$/,
                use: [{
                    loader: "url-loader",
                    options: {
                        limit: 6144,
                        name: "[name].[ext]",
                        outputPath: relativeOutputPath + "/style/assets/",
                        publicPath: "assets/"
                    }
                }]
            }
            ]
        },
        node: {
            fs: 'empty'
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ]
    });

    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = projectPath.clientOutputPath;
    const clientBundleConfig = merge(sharedConfig(), {
        entry: entry,
        module: {
            rules: [{
                test: /\.s?[ac]ss$/,
                use: [{
                    loader: 'style-loader',
                },
                {
                    loader: MiniCssExtractPlugin.loader,
                },
                {
                    loader: 'css-loader',
                }, {
                    loader: 'resolve-url-loader',
                }, {
                    loader: 'sass-loader',
                    options: {
                        sourceMap: true,
                        sourceMapContents: false
                    }
                }
                ]
            }]
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: "[name].css"
            }),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require(`${clientBundleOutputDir}/vendors/react-manifest.json`)
            }),
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require(`${clientBundleOutputDir}/vendors/moment-manifest.json`)
            })
        ].concat(isDevBuild ? [] : [new OptimizeCSSAssetsPlugin(), new CleanWebpackPlugin(pathsToClean, cleanOptions),]),
        devtool: isDevBuild ? "source-map" : undefined
    });

    const layoutBundleConfig = merge(sharedConfig(), {
        entry: layoutEntry,
        devtool: isDevBuild ? "source-map" : undefined
    });

    return [clientBundleConfig, layoutBundleConfig];
};