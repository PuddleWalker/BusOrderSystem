CREATE DATABASE BusOrderSystem;

CREATE TABLE BusModels (
    ModelId INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(100) NOT NULL UNIQUE,
    ManufacturerCompany NVARCHAR(100),
    ManufacturerCountry NVARCHAR(50),   
    Capacity INT NOT NULL CHECK (Capacity > 0),
    Price INT NOT NULL CHECK (Price > 0),
);


CREATE TABLE BusFleets (
    FleetId INT IDENTITY(1,1) PRIMARY KEY,
    FleetName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    ContactPhone NVARCHAR(20),
    Email NVARCHAR(100),
    HourlyRate DECIMAL(10, 6) NOT NULL,
    KilometerRate DECIMAL(10, 3) NOT NULL,
    Region NVARCHAR(100) NOT NULL,
);


CREATE TABLE Shifts (
    ShiftId INT IDENTITY(1,1) PRIMARY KEY,
    FleetId INT NOT NULL,
    ShiftName NVARCHAR(50) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,

    CONSTRAINT FK_Shifts_Fleet FOREIGN KEY (FleetId) REFERENCES BusFleets(FleetId) ON DELETE CASCADE,
    CONSTRAINT CK_Shift_Times CHECK (StartTime < EndTime)
);


CREATE TABLE Buses (
    BusId INT IDENTITY(1,1) PRIMARY KEY,
    BusNumber NVARCHAR(20) NOT NULL UNIQUE,
    ModelId INT NOT NULL,
    FleetId INT NOT NULL,
    Mileage DECIMAL(10,2) NOT NULL DEFAULT 0,

    CONSTRAINT FK_Buses_Model FOREIGN KEY (ModelId) REFERENCES BusModels(ModelId),
    CONSTRAINT FK_Buses_Fleet FOREIGN KEY (FleetId) REFERENCES BusFleets(FleetId)
);


CREATE TABLE Drivers (
    DriverId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    FleetId INT NOT NULL,
    EmploymentDate DATE DEFAULT GETDATE(),
    ExperienceYears INT NOT NULL,

    CONSTRAINT FK_Drivers_Garage FOREIGN KEY (FleetId) REFERENCES BusFleets(FleetId)
);

CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100)
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    BusId INT NOT NULL,
    CustomerId INT NOT NULL,
    DriverId INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    DistanceKm DECIMAL(10,2) DEFAULT 0,

    CONSTRAINT FK_Orders_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_Orders_Driver FOREIGN KEY (DriverId) REFERENCES Drivers(DriverId),
    CONSTRAINT FK_Orders_Customer FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);


CREATE INDEX IX_Buses_FleetId ON Buses(FleetId);
CREATE INDEX IX_Buses_ModelId ON Buses(ModelId);
CREATE INDEX IX_Drivers_FleetId ON Drivers(FleetId);
CREATE INDEX IX_Orders_StartTime ON Orders(StartTime);
CREATE INDEX IX_Orders_BusId ON Orders(BusId);
CREATE INDEX IX_Orders_DriverId ON Orders(DriverId);
CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);