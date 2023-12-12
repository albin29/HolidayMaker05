using HolidayMaker05;
using Npgsql;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=holidaymaker;";
await using var db = NpgsqlDataSource.Create(dbUri);

await ActivateTables();
Admin admin = new Admin();

admin.Menu();

async Task ActivateTables()
{ 
    string qHotels = @"create table if not exists hotel (
    hotel_id                serial primary key,
    name                    text,
    rating                  int;)";

    string qRooms = @"create table if not exists room (
    room_id                 serial primary key,
    number                  int,
    beds                    int,
    price                   int;)";

    string qGuests = @"create table if not exists guest (
    guest_id                serial primary key,
    name                    text,
    last_name               text, 
    email                   text, 
    phone_number            int);";

    string qLocations = @"create table if not exists locations(
    location_id             serial primary key,
    name                    text,
    location_id             serial references locations (location_id)),
    distance_from_central   int,
    distance_from_beach     int;";

    string qReservations = @"create table if not exists reservation (
    reservation_id          serial primary key,
    constraint              hotel_id foreign key(hotel)         references hotel(hotel_id),
    constraint              room_id foreign key(room)           references room(room_id),
    constraint              guest_id foreign key(guest)         references guest(guest_id),
    starting_date           date,
    ending_date             date);";

    string qRoomAddons = @"create table if not exists room_addons(
    room_id                 room_id foreign(room)               references room(room_id),
    balcony                 bool
    ac                      bool
    jacuzzi                 bool
    smart_tv                bool)";

    string qHotelAddons = @"create table if not exists hotel_addons(
    room_id                 room_id foreign(room)               references room(room_id),
    live_performance        bool
    pool                    bool
    childrens_club          bool
    restaurant              bool);";

    await db.CreateCommand(qHotels).ExecuteNonQueryAsync();
    await db.CreateCommand(qRooms).ExecuteNonQueryAsync();
    await db.CreateCommand(qLocations).ExecuteNonQueryAsync();
    await db.CreateCommand(qRoomAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qHotelAddons).ExecuteNonQueryAsync();
    await db.CreateCommand(qReservations).ExecuteNonQueryAsync();
    await db.CreateCommand(qGuests).ExecuteNonQueryAsync();
}