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

// Profil-ID erhalten
$profileId = (int)$_GET['profile_id'];

// Profil löschen
$stmt = $conn->prepare("DELETE FROM profiles WHERE id = ?");
$stmt->bind_param("i", $profileId);

if ($stmt->execute()) {
    $response = ["status" => "success"];
} else {
    $response = ["status" => "error", "message" => "Profil konnte nicht gelöscht werden"];
}

$stmt->close();
$conn->close();

echo json_encode($response);
?>