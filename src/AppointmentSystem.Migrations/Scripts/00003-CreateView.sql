-- Drop the view if it exists to avoid conflicts
DROP VIEW IF EXISTS vw_matching_available_slots;

-- Create the view to show available slots without filtering by languages, products, or ratings
CREATE VIEW vw_matching_available_slots AS
SELECT
    s.id AS slot_id,
    s.start_date,
    s.end_date,
    s.sales_manager_id,
    sm.name AS sales_manager_name,
    sm.languages,
    sm.products,
    sm.customer_ratings
FROM
    slots s
        JOIN
    sales_managers sm ON s.sales_manager_id = sm.id
WHERE
    NOT s.booked
  -- Ensure no conflict with other booked slots for the same sales manager
  AND NOT EXISTS (
    SELECT 1
    FROM slots booked_slots
    WHERE
        booked_slots.sales_manager_id = s.sales_manager_id
      AND booked_slots.booked
      AND s.start_date < booked_slots.end_date
      AND s.end_date > booked_slots.start_date
);
