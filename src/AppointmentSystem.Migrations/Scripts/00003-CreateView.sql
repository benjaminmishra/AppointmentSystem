-- Drop the view if it exists
DROP VIEW IF EXISTS vw_available_slots;

-- Create the view
CREATE VIEW vw_available_slots AS
SELECT
    s.id AS SlotId,
    s.start_date AS StartDate,
    s.end_date AS EndDate,
    sm.id AS SalesManagerId
FROM
    slots s
        LEFT JOIN
    sales_managers sm ON sm.id = s.sales_manager_id
WHERE
    NOT s.booked
  AND NOT EXISTS (
    SELECT 1
    FROM slots booked_slots
    WHERE
        booked_slots.sales_manager_id = s.sales_manager_id
      AND booked_slots.booked
      AND s.start_date < booked_slots.end_date
      AND s.end_date > booked_slots.start_date
);