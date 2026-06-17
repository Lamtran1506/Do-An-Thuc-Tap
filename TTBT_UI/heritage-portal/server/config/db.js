const mysql = require('mysql2/promise');
require('dotenv').config();

// Pool kết nối MySQL - dùng chung cho toàn bộ ứng dụng
const pool = mysql.createPool({
  host: process.env.DB_HOST || 'localhost',
  user: process.env.DB_USER || 'root',
  password: process.env.DB_PASSWORD || '',
  database: process.env.DB_NAME || 'heritage_portal',
  waitForConnections: true,
  connectionLimit: 10,
  queueLimit: 0,
});

module.exports = pool;
