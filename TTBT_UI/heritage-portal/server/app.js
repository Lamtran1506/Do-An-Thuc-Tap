const express = require('express');
const session = require('express-session');
const cors = require('cors');
require('dotenv').config();

const authRoutes = require('./routes/auth');
const reportRoutes = require('./routes/reports');

const app = express();

// Cho phép frontend (React, chạy ở cổng khác) gọi API kèm cookie session
app.use(
  cors({
    origin: process.env.CLIENT_URL || 'http://localhost:3000',
    credentials: true,
  })
);

app.use(express.json());

// Session lưu thông tin đăng nhập + quyền của user
app.use(
  session({
    secret: process.env.SESSION_SECRET || 'change_this_secret',
    resave: false,
    saveUninitialized: false,
    cookie: {
      httpOnly: true,
      maxAge: 1000 * 60 * 60 * 8, // 8 giờ
      sameSite: 'lax',
    },
  })
);

// Routes
app.use('/api/auth', authRoutes);
app.use('/api/reports', reportRoutes);

// Route kiểm tra server sống
app.get('/api/health', (req, res) => {
  res.json({ status: 'ok' });
});

// Middleware xử lý lỗi chung
app.use((err, req, res, next) => {
  console.error(err);
  res.status(500).json({ message: 'Lỗi server', error: err.message });
});

const PORT = process.env.PORT || 5000;
app.listen(PORT, () => {
  console.log(`Server đang chạy tại http://localhost:${PORT}`);
});
