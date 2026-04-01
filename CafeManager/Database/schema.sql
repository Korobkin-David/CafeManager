-- role
CREATE TABLE roles (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(25) NOT NULL
);
-- users
CREATE TABLE users (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    surname VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL,
    passwordhash VARCHAR(255) NOT NULL,
    role_id INT NOT NULL,
    FOREIGN KEY (role_id) REFERENCES roles(id)
);

-- sklad (ingredience pro menu)
CREATE TABLE ingredients (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    unit VARCHAR(20) NOT NULL,       -- kg, l, ks...
    quantity DECIMAL(10,3) NOT NULL DEFAULT 0,
    min_quantity DECIMAL(10,3) NOT NULL DEFAULT 0  -- práh upozornění (pak se bude odesílat upozornění)
);

-- kategorie v menu
CREATE TABLE categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50)
);

-- menu
CREATE TABLE menu (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(100) NOT NULL,
    price DECIMAL(8,2) NOT NULL,
    category_id INT,
    is_available TINYINT(1) DEFAULT 1,
    FOREIGN KEY (category_id) REFERENCES categories(id)
);

-- Stoly
CREATE TABLE tables_cafe (
     id INT AUTO_INCREMENT PRIMARY KEY,
     number INT NOT NULL,
     capacity INT NOT NULL DEFAULT 4,
     status ENUM('free','occupied') DEFAULT 'free'
);

-- Objednávky
CREATE TABLE orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    table_id INT NOT NULL,
    user_id INT NOT NULL,
    created_at DATETIME DEFAULT NOW(),
    closed_at DATETIME,
    status ENUM('open','closed','cancelled') DEFAULT 'open',
    total DECIMAL(8,2) DEFAULT 0,
    FOREIGN KEY (table_id) REFERENCES tables_cafe(id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Objednávky - položky
CREATE TABLE order_items (
     id INT AUTO_INCREMENT PRIMARY KEY,
     order_id INT NOT NULL,
     menu_id INT NOT NULL,
     quantity INT NOT NULL DEFAULT 1,
     unit_price DECIMAL(8,2) NOT NULL,
     FOREIGN KEY (order_id) REFERENCES orders(id),
     FOREIGN KEY (menu_item_id) REFERENCES menu(id)
);
