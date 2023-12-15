using HolidayMaker05;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker;";
await using var db = NpgsqlDataSource.Create(dbUri);

await ActivateTables();
Admin admin = new Admin(db);

await admin.Menu();

async Task ActivateTables()
{
    string qHotels = @"create table if not exists hotels(
    hotel_id                serial primary key,
    name                    text,
    rating                  int,
    location_id             int references locations(location_id),
    hotel_addon_id          int references hotel_addons(hotel_addon_id));";

    string qRooms = @"create table if not exists rooms(
    room_id                 serial primary key,
    number                  int,
    hotel_id                int references hotels(hotel_id),
    beds                    int,
    price                   int,
    room_addon_id           int references room_addons(room_addon_id));";

    string qLocations = @"create table if not exists locations(
    location_id             serial primary key,
    address                 text,
    distance_from_central   real,
    distance_from_beach     real);";

    string qReservations = @"create table if not exists reservations (
    reservation_id          serial primary key,
    room_id                 int references rooms(room_id),
    full_name               text,
    email                   text,
    starting_date           date,
    ending_date             date);";

    string qRoomAddons = @"create table if not exists room_addons(
    room_addon_id           serial primary key,
    balcony                 boolean,
    ac                      boolean,
    jacuzzi                 boolean,
    smart_tv                boolean);";

    string qHotelAddons = @"create table if not exists hotel_addons(
    hotel_addon_id          serial primary key,
    live_performance        boolean,
    pool                    boolean,
    childrens_club          boolean,
    restaurant              boolean);";

    await db.CreateCommand(qLocations).ExecuteNonQueryAsync();
    await db.CreateCommand(qHotelAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qRoomAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qHotels).ExecuteNonQueryAsync();
    await db.CreateCommand(qRooms).ExecuteNonQueryAsync();
    await db.CreateCommand(qReservations).ExecuteNonQueryAsync();

    string locationData = @"
    INSERT INTO locations(address, distance_from_central, distance_from_beach)
    VALUES
    ('123 Main Street, Anytown, USA', 5.4, 10),
    ('456 Elm Avenue, Somewhere City, USA', 8.1, 4.3),
    ('789 Oak Lane, Anyville, USA', 12.0, 3.9),
    ('101 Maple Drive, Nowhereville, USA', 7, 1.2),
    ('321 Pine Boulevard, Another Town, USA', 1.4, 1.8);";

    string hotelData = @"
    INSERT INTO hotels(name, rating, location_id, hotel_addon_id)
    VALUES
    ('bellagio', 4, 1, 1),
    ('the_ritz', 5, 2, 2),
    ('the_hilton', 4, 3, 4),
    ('aura_hotel', 3, 4, 5),
    ('the_dagbah', 5, 5, 3);";


    string hotelAddonData = @"
    INSERT INTO hotel_addons(live_performance, pool, childrens_club, restaurant)
    VALUES
    (true, true, false, true),
    (false, true, true, false),
    (false, true, true, false),
    (false, true, true, false),
    (true, false, true, true);";

    string roomAddonData = @"
    INSERT INTO room_addons(balcony, ac, jacuzzi, smart_tv)
    VALUES
    (true, true, false, true),
    (false, true, true, false),
    (true, true, true, true),
    (true, true, false, false),
    (false, true, false, true),
    (true, false, true, true),
    (false, true, true, true);";

    string roomData = @"
    INSERT INTO rooms(number, hotel_id, beds, price, room_addon_id)
    VALUES
    (101, 1, 1, 400, 1),
    (102, 1, 3, 450, 2),
    (103, 1, 2, 400, 3),
    (104, 1, 3, 550, 3),
    (201, 1, 2, 500, 6),
    (202, 1, 3, 550, 6),
    (203, 1, 2, 500, 4),
    (204, 1, 3, 550, 7),
    (301, 1, 3, 800, 7),
    (302, 1, 2, 600, 3),
    (101, 1, 2, 500, 4),
    (101, 2, 2, 550, 4),
    (102, 2, 3, 500, 1),
    (103, 2, 3, 550, 3),
    (201, 2, 1, 400, 5),
    (202, 2, 1, 450, 7),
    (203, 2, 3, 400, 7),
    (101, 3, 3, 450, 6),
    (102, 3, 3, 550, 1),
    (103, 3, 1, 500, 1),
    (104, 3, 1, 400, 2),
    (205, 3, 2, 450, 2),
    (101, 3, 2, 400, 3),
    (202, 3, 2, 450, 4),
    (103, 3, 3, 500, 5),
    (204, 3, 1, 550, 2),
    (105, 3, 1, 500, 7),
    (201, 3, 2, 550, 2),
    (302, 3, 3, 400, 5),
    (303, 3, 3, 400, 3),
    (104, 3, 1, 400, 5),
    (205, 3, 1, 750, 4),
    (101, 4, 2, 850, 4),
    (102, 4, 2, 750, 5),
    (103, 4, 2, 800, 7),
    (104, 4, 3, 650, 2),
    (201, 4, 3, 700, 1),
    (202, 4, 2, 600, 1),
    (203, 4, 2, 600, 1),
    (204, 4, 3, 650, 5),
    (301, 4, 3, 850, 4),
    (302, 4, 2, 850, 4),
    (303, 4, 1, 700, 6),
    (304, 4, 3, 700, 6),
    (401, 4, 3, 700, 6),
    (402, 4, 3, 750, 5),
    (403, 4, 1, 850, 4),
    (404, 4, 1, 800, 7),
    (101, 5, 5, 1450, 4),
    (201, 5, 5, 1650, 5),
    (301, 5, 6, 1850, 5);";

    /*
    await db.CreateCommand(hotelAddonData).ExecuteNonQueryAsync();
    await db.CreateCommand(locationData).ExecuteNonQueryAsync();
    await db.CreateCommand(hotelData).ExecuteNonQueryAsync();
    await db.CreateCommand(roomAddonData).ExecuteNonQueryAsync();
    await db.CreateCommand(roomData).ExecuteNonQueryAsync();
    */
}