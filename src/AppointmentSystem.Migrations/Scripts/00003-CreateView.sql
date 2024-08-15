DROP FUNCTION IF EXISTS fn_matching_available_slots(
    langs varchar[],
    ratings varchar[],
    prods varchar[]
);

CREATE FUNCTION fn_matching_available_slots(
    langs varchar[],
    ratings varchar[],
    prods varchar[]
)
    RETURNS TABLE (
                      SlotId int,
                      StartDate timestamptz,
                      EndDate timestamptz,
                      SalesManagerId int,
                      SalesManagerName varchar,
                      Languages varchar[],
                      Products varchar[],
                      CustomerRatings varchar[]
                  ) AS $$
BEGIN
    RETURN QUERY
        SELECT
            s.id AS SlotId,
            s.start_date AS StartDate,
            s.end_date AS EndDate,
            s.sales_manager_id AS SalesManagerId,
            sm.name AS SalesManagerName,
            sm.languages AS Languages,
            sm.products AS Products,
            sm.customer_ratings AS CustomerRatings
        FROM
            slots s
                JOIN
            sales_managers sm ON s.sales_manager_id = sm.id
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
        )
          AND sm.languages && langs  -- Array overlap for languages
          AND sm.customer_ratings && ratings  -- Array overlap for ratings
          AND sm.products && prods;  -- Array overlap for products
END;
$$ LANGUAGE plpgsql;
