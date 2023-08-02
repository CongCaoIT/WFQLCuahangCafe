CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

CREATE TABLE BAN 
(
    MABAN INT IDENTITY NOT NULL,
    TEN NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    TRANGTHAI NVARCHAR(100) NOT NULL DEFAULT N'Trống', -- Trống or Có người
   
    CONSTRAINT PK_TF PRIMARY KEY (MABAN)
)
GO

CREATE TABLE TAIKHOAN
(
    TENDANGNHAP NVARCHAR(100) NOT NULL,
    TENHIENTHI NVARCHAR(100) NOT NULL,
    MATKHAU NVARCHAR(1000) NOT NULL, -- nhớ set default matkhau
    LOAITAIKHOAN INT NOT NULL DEFAULT 0, -- 1 ADMIN -- 0 NGƯỜI DÙNG
    
    CONSTRAINT PK_AC PRIMARY KEY (TENDANGNHAP)
)
GO

CREATE TABLE LOAIMONAN
(
    MALOAIMONAN INT IDENTITY NOT NULL,
    TEN NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    
    CONSTRAINT PK_FC PRIMARY KEY (MALOAIMONAN)
)
GO

CREATE TABLE MONAN
(
    MAMONAN INT IDENTITY NOT NULL,
    TEN NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    MALOAIMONAN INT NOT NULL,
    GIA MONEY NOT NULL DEFAULT 0,
    
    CONSTRAINT PK_F PRIMARY KEY (MAMONAN),
    CONSTRAINT FK_F_FC FOREIGN KEY (MALOAIMONAN) REFERENCES LOAIMONAN(MALOAIMONAN)
)
GO

CREATE TABLE HOADON
(
    MAHOADON INT IDENTITY NOT NULL,
    NGAYVAO DATE NOT NULL DEFAULT GETDATE(),
    NGAYRA DATE,
    MABAN INT NOT NULL,
    TRANGTHAI INT DEFAULT 0, -- 1 Đã thanh toán, 0 chưa thanh toán
    GIAMGIA INT DEFAULT 0, 
    TONGTHANHTIEN MONEY 
    
    CONSTRAINT PK_B PRIMARY KEY (MAHOADON),
    CONSTRAINT FK_B_TABLE FOREIGN KEY (MABAN) REFERENCES BAN(MABAN)
)
GO

CREATE TABLE CHITIETHD
(
    MACHITETHD INT IDENTITY NOT NULL,
    MAHOADON INT NOT NULL,
    MAMONAN INT NOT NULL,
    COUNT INT NOT NULL DEFAULT 0,
   
    CONSTRAINT PK_BIF PRIMARY KEY (MACHITETHD),
    CONSTRAINT FK_BIF_B FOREIGN KEY (MAHOADON) REFERENCES HOADON(MAHOADON),
    CONSTRAINT FK_BIF_FOOD FOREIGN KEY (MAMONAN) REFERENCES MONAN(MAMONAN)
)
GO

INSERT INTO TAIKHOAN VALUES
('Nhanvien', N'Nhân viên', '1962026656160185351301320480154111117132155', 0),
('admin', 'Admin', '1962026656160185351301320480154111117132155', 1)
GO

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS
BEGIN
    SELECT * FROM TAIKHOAN WHERE TENDANGNHAP = @userName
END
GO

CREATE PROC USP_Login -- khắc phục được lỗi đó
    @userName nvarchar(100),
    @passWord nvarchar(100)
AS
BEGIN
    SELECT *
    FROM TAIKHOAN
    WHERE BINARY_CHECKSUM(TENDANGNHAP) = BINARY_CHECKSUM(@userName) AND BINARY_CHECKSUM(MATKHAU) = BINARY_CHECKSUM(@passWord);
END
GO

DECLARE @i INT = 1 -- Thêm dữ liệu theo vòng lặp
WHILE @i <= 20
BEGIN
    INSERT INTO BAN(TEN, TRANGTHAI) VALUES (N'Bàn ' + CAST(@i as nvarchar(100)), N'Trống')
    SET @i = @i+1
END
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM BAN
GO

--Thêm loại món ăn
INSERT INTO LOAIMONAN VALUES
(N'Hải sản'),
(N'Thịt'),
(N'Cá'),
(N'Ốc'),
(N'Trái cây'),
(N'Nước')
GO

--Thêm món ăn
INSERT INTO MONAN VALUES
(N'Mực một nắng nướng sa tế', 1, 150000),
(N'Hào nướng các loại', 1, 5000),
(N'Dú heo nướng', 2, 15000),
(N'Sường nướng muối ướt', 2, 20000),
(N'Cá điêu hồng hấp', 3, 100000),
(N'Lẩu cá đuối', 3, 200000),
(N'Ốc bưu nhồi thịt', 4, 60000),
(N'Óc hương cháy tỏi', 4, 40000),
(N'Trái cây các loại', 5, 50000),
(N'Nước ngọt Cocacola', 6, 15000),
(N'Nước ngọt 7 Up', 6, 15000),
(N'Nước ngọt Sting', 6, 15000)
GO

-- Thêm hóa đơn
CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
    INSERT INTO HOADON(NGAYVAO, NGAYRA, MABAN, TRANGTHAI) VALUES
    (GETDATE(), NULL, @idTable, 0)
END
GO

-- Thêm chi tiết hóa đơn
CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN

    DECLARE @isExitsBillInfo INT
    DECLARE @foodCount INT = 0

    SELECT @isExitsBillInfo = MACHITETHD, @foodCount = cthd.COUNT 
    FROM CHITIETHD as cthd WHERE MAHOADON = @idBill AND MAMONAN = @idFood 

    IF(@isExitsBillInfo > 0)
        BEGIN
            DECLARE @newCount INT = @foodCount + @count
            IF(@newCount > 0)
                UPDATE CHITIETHD SET COUNT = @foodCount + @count WHERE MAHOADON = @idBill AND MAMONAN = @idFood
            ELSE
                DELETE CHITIETHD WHERE MAHOADON = @idBill AND MAMONAN = @idFood
        END
    ELSE
        BEGIN
            INSERT INTO CHITIETHD VALUES
            (@idBill, @idFood, @count)
        END

END
GO

CREATE TRIGGER UTG_UpdateBillInfo --RÀNG BUỘC BẢN CHI TIẾT HÓA ĐƠN, SET TRẠNG THÁI CÓ NGƯỜI
ON CHITIETHD FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @idBill INT

    SELECT @idBill = MAHOADON FROM inserted

    DECLARE @idTable INT

    SELECT @idTable = MABAN FROM HOADON WhERE MAHOADON = @idBill AND TRANGTHAI = 0

    DECLARE @count INT

    SELECT  @count = COUNT(*) FROM CHITIETHD WHERE MAHOADON = @idBill

    IF(@count > 0)
        UPDATE BAN SET TRANGTHAI = N'Có người' WHERE MABAN = @idTable
    ELSE
        UPDATE BAN SET TRANGTHAI = N'Trống' WHERE MABAN = @idTable
END
GO

CREATE TRIGGER UTG_UpdateBill --RÀNG BUỘC TRẠNG THÁI CỦA HÓA ĐƠN
ON HOADON FOR UPDATE
AS
BEGIN
    DECLARE @idBill INT

    SELECT @idBill = MAHOADON FROM inserted

    DECLARE @idTable INT

    SELECT @idTable = MABAN FROM HOADON WhERE MAHOADON = @idBill 

    DECLARE @count INT = 0

    SELECT @count = COUNT(*) FROM HOADON WHERE MABAN = @idTable AND TRANGTHAI = 0

    IF(@count = 0)
        UPDATE BAN SET TRANGTHAI = N'Trống' WHERE MABAN = @idTable
END
GO

-- PROC XỬ LÝ CHUYỂN BÀN
CREATE PROC USP_SwicthTable
@idTable1 INT, @idTable2 INT
AS
BEGIN
    DECLARE @idFirstBill INT
    DECLARE @idSeconrdBill INT

    DECLARE @isFirstTableEmty INT = 1
    DECLARE @isSeconrdTableEmty INT = 1

    SELECT @idFirstBill = MAHOADON FROM HOADON WHERE MABAN = @idTable1 AND TRANGTHAI = 0 -- LẤY ID CỦA 2 HÓA ĐƠN NẾU ĐÃ CÓ BÀN
    SELECT @idSeconrdBill = MAHOADON FROM HOADON WHERE MABAN = @idTable2 AND TRANGTHAI = 0

    IF(@idFirstBill IS NULL) --THÊM HÓA ĐƠN MỚI -- SO SÁNH VỚI NULL PHẢI SỬ DỤNG IS KHÔNG ĐƯỢC DÙNG DẤU =
        BEGIN
            INSERT INTO HOADON(NGAYVAO, NGAYRA, MABAN, TRANGTHAI) VALUES (GETDATE(), NULL, @idTable1, 0)
            SELECT @idFirstBill = MAX(MAHOADON) FROM HOADON WHERE MABAN = @idTable1 AND TRANGTHAI = 0
        END
    
    SELECT @isFirstTableEmty = COUNT(*) FROM CHITIETHD WHERE MAHOADON = @idFirstBill

    IF(@idSeconrdBill IS NULL)
        BEGIN
            INSERT INTO HOADON(NGAYVAO, NGAYRA, MABAN, TRANGTHAI) VALUES (GETDATE(), NULL, @idTable2, 0)
            SELECT @idSeconrdBill = MAX(MAHOADON) FROM HOADON WHERE MABAN = @idTable2 AND TRANGTHAI = 0
        END

    SELECT @isSeconrdTableEmty = COUNT(*) FROM CHITIETHD WHERE MAHOADON = @idSeconrdBill

    SELECT MACHITETHD INTO IDBillInfoTable FROM CHITIETHD WHERE MAHOADON = @idSeconrdBill -- LẤY RA MACTHD CỦA HÓA ĐƠN 2 VÀ ADD VÀO BẢN MỚI
    UPDATE CHITIETHD SET MAHOADON = @idSeconrdBill WHERE MAHOADON = @idFirstBill -- CHUYỂN TẤT CẢ MACTHD CỦA THẰNG THỨ 1 SANG CHO THẰNG THỨ 2
    UPDATE CHITIETHD SET MAHOADON = @idFirstBill WHERE MACHITETHD IN (SELECT * FROM IDBillInfoTable) -- CHUYỂN TÁT CẢ ID TK THỨ 2 NẰM TRONG TABLE MỚI CHO TK THỬ NHẤT
    DROP TABLE IDBillInfoTable -- XÓA TABLE ĐỂ TIẾP TỤC THỰC HIỆN CODE

    IF(@isFirstTableEmty = 0)
        UPDATE BAN SET TRANGTHAI = N'Trống' WHERE MABAN = @idTable2

    IF(@isSeconrdTableEmty = 0)
        UPDATE BAN SET TRANGTHAI = N'Trống' WHERE MABAN = @idTable1
END
GO

CREATE PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS
BEGIN
    SELECT B.TEN as [Tên Bàn], NGAYVAO as [Ngày Vào], NGAYRA as [Ngày Ra], GIAMGIA as [Giảm Giá (%)], FORMAT(TONGTHANHTIEN, 'c', 'vi-VN') as [Tổng tiền hóa đơn]
    FROM HOADON HD, BAN B
    WHERE B.MABAN = HD.MABAN 
    AND NGAYVAO >= @checkIN AND NGAYRA <= @checkOut AND HD.TRANGTHAI = 1   
END
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON CHITIETHD FOR DELETE
AS
BEGIN
    DECLARE @idBill INT
    DECLARE @idBillInfo INT
    SELECT @idBillInfo = MACHITETHD, @idBill = MAHOADON FROM deleted

    DECLARE @idTable INT
    SELECT @idTable = MABAN FROM HOADON WHERE MAHOADON = @idBill

    DECLARE @count INT = 0
    SELECT @count = COUNT(*) FROM CHITIETHD CTHD, HOADON HD WHERE HD.MAHOADON = CTHD.MAHOADON AND HD.MAHOADON = @idBill AND HD.TRANGTHAI = 0

    IF(@count = 0)
        UPDATE BAN SET TRANGTHAI = N'Trống' WHERE MABAN = @idTable
END
GO

-- Function Loại bỏ dấu tiếng Việt, giữ nguyên chữ in hoa và các ký tự đặc biệt
CREATE FUNCTION [dbo].[fuConvertToUnsign1]
(
 @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN 
 IF @strInput IS NULL RETURN @strInput
 IF @strInput = '' RETURN @strInput
 DECLARE @RT NVARCHAR(4000)
 DECLARE @SIGN_CHARS NCHAR(136)
 DECLARE @UNSIGN_CHARS NCHAR (136)
 SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
 ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
 ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
 ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
 +NCHAR(272)+ NCHAR(208)
 SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
 iiiiiooooooooooooooouuuuuuuuuuyyyyy
 AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
 OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 DECLARE @COUNTER int
 DECLARE @COUNTER1 int
 SET @COUNTER = 1
 WHILE (@COUNTER <=LEN(@strInput))
 BEGIN 
 SET @COUNTER1 = 1
 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
 BEGIN
 IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1))
 = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
 BEGIN 
 IF @COUNTER=1
 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
 ELSE
 SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1)
 +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
 BREAK
 END
 SET @COUNTER1 = @COUNTER1 +1
 END
 SET @COUNTER = @COUNTER +1
 END
 SET @strInput = replace(@strInput,' ','-')
 RETURN @strInput
END
GO

--Function Loại bỏ dấu tiếng Việt, chuyển in hoa thành in thường, loại bỏ các ký tự đặc biệt
CREATE FUNCTION [dbo].[fuConvertToUnsign2]
(
 @strInput NVARCHAR(4000)
) 
RETURNS NVARCHAR(4000)
AS
Begin
 Set @strInput=rtrim(ltrim(lower(@strInput)))
 IF @strInput IS NULL RETURN @strInput
 IF @strInput = '' RETURN @strInput
 Declare @text nvarchar(50), @i int
 Set @text='-''`~!@#$%^&*()?><:|}{,./\"''='';–'
 Select @i= PATINDEX('%['+@text+']%',@strInput ) 
 while @i > 0
 begin
 set @strInput = replace(@strInput, substring(@strInput, @i, 1), '')
 set @i = patindex('%['+@text+']%', @strInput)
 End
 Set @strInput =replace(@strInput,' ',' ')
 
 DECLARE @RT NVARCHAR(4000)
 DECLARE @SIGN_CHARS NCHAR(136)
 DECLARE @UNSIGN_CHARS NCHAR (136)
 SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
 ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý'
 +NCHAR(272)+ NCHAR(208)
 SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
 iiiiiooooooooooooooouuuuuuuuuuyyyyy'
 DECLARE @COUNTER int
 DECLARE @COUNTER1 int
 SET @COUNTER = 1
 WHILE (@COUNTER <=LEN(@strInput))
 BEGIN 
 SET @COUNTER1 = 1
 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
 BEGIN
 IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) 
 = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
 BEGIN 
 IF @COUNTER=1
 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) 
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
 ELSE
 SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) 
 +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) 
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
 BREAK
 END
 SET @COUNTER1 = @COUNTER1 +1
 END
 SET @COUNTER = @COUNTER +1
 End
 SET @strInput = replace(@strInput,' ','-')
 RETURN lower(@strInput)
End
GO

CREATE PROC USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
AS
BEGIN
    declare @pageRows int = 10
    declare @selectRows int = @pageRows 
    declare @exceptRows int = (@page - 1) * @pageRows
    
    ;WiTH BillShow as (SELECT HD.MAHOADON, B.TEN as [Tên Bàn], NGAYVAO as [Ngày Vào], NGAYRA as [Ngày Ra], GIAMGIA as [Giảm Giá (%)], FORMAT(TONGTHANHTIEN, 'c', 'vi-VN') as [Tổng tiền hóa đơn]
    FROM HOADON HD, BAN B
    WHERE B.MABAN = HD.MABAN 
    AND NGAYVAO >= @checkIN AND NGAYRA <= @checkOut AND HD.TRANGTHAI = 1)  

    SELECT TOP (@selectRows) * FROM BillShow WHERE MAHOADON NOT IN (SELECT TOP (@exceptRows) MAHOADON FROM BillShow)

END
GO

CREATE PROC USP_GetNumBillByDate
@checkIn date, @checkOut date
AS
BEGIN
    SELECT  COUNT(*)
    FROM HOADON HD, BAN B
    WHERE B.MABAN = HD.MABAN 
    AND NGAYVAO >= @checkIN AND NGAYRA <= @checkOut AND HD.TRANGTHAI = 1   
END
GO
