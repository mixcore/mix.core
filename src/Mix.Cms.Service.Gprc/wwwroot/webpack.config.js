const path = require('path');

module.exports = {
  entry: './Scripts/index.js',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'mix-gprc.min.js'
  }
};