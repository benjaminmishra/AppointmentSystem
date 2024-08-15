-- Insert 5000 sales managers
INSERT INTO sales_managers (name, languages, products, customer_ratings)
SELECT
    'Seller ' || seq,
    ARRAY(
            SELECT unnest(ARRAY['English', 'German', 'French', 'Spanish', 'Italian'])
            ORDER BY random()
            LIMIT (1 + floor(random() * 3))::int
    ),
    ARRAY(
            SELECT unnest(ARRAY['SolarPanels', 'Heatpumps', 'WindTurbines', 'Batteries'])
            ORDER BY random()
            LIMIT (1 + floor(random() * 3))::int
    ),
    ARRAY(
            SELECT unnest(ARRAY['Bronze', 'Silver', 'Gold', 'Platinum'])
            ORDER BY random()
            LIMIT (1 + floor(random() * 4))::int
    )
FROM generate_series(1, 5000) AS seq;

-- Insert 100,000 slots
INSERT INTO slots (sales_manager_id, booked, start_date, end_date)
SELECT
    (SELECT id FROM sales_managers ORDER BY random() LIMIT 1) AS sales_manager_id,
    (random() > 0.5) AS booked,
    start_time AS start_date,
    start_time + interval '1 hour' AS end_date
FROM (
         SELECT
             '2024-05-03 08:00:00+00'::timestamptz + (floor(random() * 43200) * '1 minute'::interval) AS start_time
         FROM generate_series(1, 100000) AS seq
     ) AS times;
