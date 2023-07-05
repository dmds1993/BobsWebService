USE BOBS_SERVICE;

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        CategoryId INT PRIMARY KEY IDENTITY(1,1),
        CategoryName VARCHAR(100) NOT NULL,
        ParentCategoryId INT,
        [Level] [int] NOT NULL,
        CONSTRAINT FK_Categories_ParentCategory
            FOREIGN KEY (ParentCategoryId)
            REFERENCES Categories (CategoryId),
        CONSTRAINT UQ_Categories_CategoryName
            UNIQUE (CategoryName)
    );

    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY,
        UserName NVARCHAR(255) NOT NULL,
        HashedPassword NVARCHAR(255) NOT NULL
    );
END
ELSE
BEGIN
    PRINT 'Database already exists.';
END
