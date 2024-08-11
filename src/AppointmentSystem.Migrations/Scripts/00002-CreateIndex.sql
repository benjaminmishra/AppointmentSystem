-- Create index on sales_manager_id in slots table to optimize JOIN operations
CREATE INDEX IF NOT EXISTS idx_slots_sales_manager_id ON slots(sales_manager_id);

-- Create composite index on start_date and end_date in slots table for range queries
CREATE INDEX IF NOT EXISTS idx_slots_start_date_end_date ON slots(start_date, end_date);

-- Create index on booked in slots table to optimize filtering on booking status
CREATE INDEX IF NOT EXISTS idx_slots_booked ON slots(booked);

-- Create GIN index on languages array in sales_managers table to optimize language filtering
CREATE INDEX IF NOT EXISTS idx_sales_managers_languages ON sales_managers USING GIN(languages);

-- Create GIN index on products array in sales_managers table to optimize product filtering
CREATE INDEX IF NOT EXISTS idx_sales_managers_products ON sales_managers USING GIN(products);

-- Create GIN index on customer_ratings array in sales_managers table to optimize rating filtering
CREATE INDEX IF NOT EXISTS idx_sales_managers_customer_ratings ON sales_managers USING GIN(customer_ratings);
