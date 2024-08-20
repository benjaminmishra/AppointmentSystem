-- sales_managers indexs
CREATE INDEX idx_sales_managers_id ON sales_managers(id);

CREATE INDEX idx_sales_managers_languages ON sales_managers USING GIN(languages);

CREATE INDEX idx_sales_managers_products ON sales_managers USING GIN(products);

CREATE INDEX idx_sales_managers_customer_ratings ON sales_managers USING GIN(customer_ratings);

-- slots indexs
CREATE INDEX idx_slots_id ON slots(id);

CREATE INDEX idx_slots_sales_manager_id ON slots(sales_manager_id);

CREATE INDEX idx_slots_date_range ON slots(start_date, end_date);

CREATE INDEX idx_slots_booked ON slots(booked);
