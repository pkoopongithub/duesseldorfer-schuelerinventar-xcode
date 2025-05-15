<?php
header('Content-Type: application/json');

// Datenbankkonfiguration
$dbhost = "localhost";
$dbname = "personality_test";
$dbuser = "db_user";
$dbpass = "secure_password";

// Verbindung herstellen
$conn = new mysqli($dbhost, $dbuser, $dbpass, $dbname);

if ($conn->connect_error) {
    die(json_encode(["status" => "error", "message" => "Datenbankverbindung fehlgeschlagen"]));
}

// Normtabelle abfragen
$query = "SELECT kompetenzID, p1, p2, p3, p4, p5 FROM normSEhs ORDER BY kompetenzID";
$result = $conn->query($query);

if ($result->num_rows === 0) {
    echo json_encode(["status" => "error", "message" => "Keine Normdaten gefunden"]);
    exit;
}

$normTable = [];
while ($row = $result->fetch_assoc()) {
    $normTable[] = [
        (float)$row['p1'],
        (float)$row['p2'],
        (float)$row['p3'],
        (float)$row['p4'],
        (float)$row['p5']
    ];
}

$response = [
    "status" => "success",
    "data" => $normTable
];

$conn->close();
echo json_encode($response);
?>