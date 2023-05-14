const path = require('path');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
  entry: {
    main: "./src/main-app.ts",
  },
  output: {
    path: path.resolve(__dirname, 'build'),
    filename: "[name]-bundle.js",
    clean: true,
  },
  resolve: {
    // Add ".ts" and ".tsx" as resolvable extensions.
    extensions: [".ts", ".tsx", ".js"],
  },
  module: {
    rules: [
      // all files with a `.ts` or `.tsx` extension will be handled by `ts-loader`
      { test: /\.tsx?$/, loader: "ts-loader" },
    ],
  },
  performance: {
    hints: false
  },
  plugins: [
    new CopyWebpackPlugin({
      patterns: [
        "index.html",
        "favicon.ico",
        {
          from: "assets/**/*",
        },
      ]
    }),
  ],
};