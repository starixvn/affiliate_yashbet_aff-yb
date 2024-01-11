const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const merge = require("webpack-merge");
const OptimizeCssAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");

const { environment, projectPath } = require("./config-utils/constant");

let pathsToClean = ["dist/vendors/*.*"];

let cleanOptions = {
    root: path.resolve(__dirname, "."),
    verbose: true,
    dry: false
};

module.exports = env => {
    env = environment[env] || environment.development;
    const isDevBuild = !(env === environment.production);
    const relativeOutputRootPath = projectPath.clientOutputPath;

    const sharedConfig = {
        context: path.resolve(__dirname, "."),
        mode: env,
        stats: { modules: false },
        module: {
            rules: [{
                test: /\.(png|woff|woff2|eot|ttf)(\?|$)/,
                use: [{
                    loader: "url-loader",
                    options: {
                        limit: 4096,
                        name: "[name].[ext]"
                    }
                }]
            }]
        },
        entry: {
            react: ["react", "react-dom", "react-redux", "redux", "redux-thunk", "reactstrap"],
            bootstrap: ["bootstrap/dist/css/bootstrap.css"],
            moment: ["moment"]
        },
        output: {
            publicPath: "dist/vendors",
            filename: "[name].js",
            library: "[name]_[hash]"
        },
        plugins: [
            // Ignore all locale files of moment.js
            new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
            new CleanWebpackPlugin(pathsToClean, cleanOptions),
            new webpack.DefinePlugin({
                "process.env.NODE_ENV": isDevBuild ? '"development"' : '"production"'
            }),
            new OptimizeCssAssetsPlugin()
        ]
    };

    const clientBundleConfig = merge(sharedConfig, {
        output: { path: `${relativeOutputRootPath}/vendors` },
        module: {
            rules: [{
                test: /\.s?[ac]ss$/,
                use: [MiniCssExtractPlugin.loader, "css-loader", "sass-loader"]
            }]
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: "[name].css"
            }),
            new webpack.DllPlugin({
                path: path.join(relativeOutputRootPath, "vendors", "[name]-manifest.json"),
                name: "[name]_[hash]"
            })
        ]
    });

    return [clientBundleConfig];
};