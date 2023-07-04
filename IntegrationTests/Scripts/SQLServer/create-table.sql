USE BOBS_SERVICE;

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        CategoryId INT PRIMARY KEY IDENTITY(1,1),
        CategoryName VARCHAR(100) NOT NULL,
        ParentCategoryId INT,
        CONSTRAINT FK_Categories_ParentCategory
            FOREIGN KEY (ParentCategoryId)
            REFERENCES Categories (CategoryId),
        CONSTRAINT UQ_Categories_CategoryName
            UNIQUE (CategoryName)
    );
    PRINT 'Database created successfully.';
END
ELSE
BEGIN
    PRINT 'Database already exists.';
END
