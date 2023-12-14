using HolidayMaker05;
using Npgsql;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker;";
await using var db = NpgsqlDataSource.Create(dbUri);

await ActivateTables();
Admin admin = new Admin(db);

admin.Menu();

async Task ActivateTables()
{
    string qHotels = @"create table if not exists hotels(
    hotel_id                serial primary key,
    name                    text,
    rating                  int,
    location_id             int references locations(location_id),
    hotel_addons            int references hotel_addons(hotel_addons_id));";

    string qRooms = @"create table if not exists rooms(
    room_id                 serial primary key,
    number                  int,
    beds                    int,
    price                   int,
    room_addon_id           int references room_addons(room_addon_id));";

    string qGuests = @"create table if not exists guests(
    guest_id                serial primary key,
    name                    text,
    last_name               text, 
    email                   text, 
    phone_number            int);";

    string qLocations = @"create table if not exists locations(
    location_id             serial primary key,
    name                    text,
    distance_from_central   int,
    distance_from_beach     int);";

    string qReservations = @"create table if not exists reservations (
    reservation_id          serial primary key,
    hotel_id                int references hotels (hotel_id),
    room_id                 int references rooms(room_id),
    guest_id                int references guests(guest_id),
    starting_date           date,
    ending_date             date);";

    string qRoomAddons = @"create table if not exists room_addons(
    room_addon_id           serial primary key,
    balcony                 boolean,
    ac                      boolean,
    jacuzzi                 boolean,
    smart_tv                boolean);";

    string qHotelAddons = @"create table if not exists hotel_addons(
    hotel_addons_id         serial primary key,
    live_performance        boolean,
    pool                    boolean,
    childrens_club          boolean,
    restaurant              boolean);";

    await db.CreateCommand(qLocations).ExecuteNonQueryAsync();
    await db.CreateCommand(qHotelAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qRoomAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qHotels).ExecuteNonQueryAsync();
    await db.CreateCommand(qRooms).ExecuteNonQueryAsync();
    await db.CreateCommand(qGuests).ExecuteNonQueryAsync();
    await db.CreateCommand(qReservations).ExecuteNonQueryAsync();
    }