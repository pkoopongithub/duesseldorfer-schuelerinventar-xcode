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

// Benutzer-ID erhalten
$userId = (int)$_GET['user_id'];

// Profile abfragen
$stmt = $conn->prepare("SELECT id, name, created_at FROM profiles WHERE user_id = ? ORDER BY created_at DESC");
$stmt->bind_param("i", $userId);
$stmt->execute();
$result = $stmt->get_result();

$profiles = [];
while ($row = $result->fetch_assoc()) {
    $profiles[] = $row;
}

$response = [
    "status" => "success",
    "data" => $profiles
];

$stmt->close();
$conn->close();

echo json_encode($response);
?>