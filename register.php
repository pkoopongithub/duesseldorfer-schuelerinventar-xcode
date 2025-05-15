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

// Eingabedaten erhalten
$json = file_get_contents('php://input');
$data = json_decode($json, true);

if (json_last_error() !== JSON_ERROR_NONE || !is_array($data)) {
    echo json_encode(["status" => "error", "message" => "Ungültige Eingabedaten"]);
    exit;
}

$username = $conn->real_escape_string($data['username']);
$email = $conn->real_escape_string($data['email']);
$password = $data['password'];

// Überprüfen, ob Benutzername oder E-Mail bereits existieren
$checkStmt = $conn->prepare("SELECT id FROM users WHERE username = ? OR email = ?");
$checkStmt->bind_param("ss", $username, $email);
$checkStmt->execute();
$checkResult = $checkStmt->get_result();

if ($checkResult->num_rows > 0) {
    echo json_encode(["status" => "error", "message" => "Benutzername oder E-Mail bereits vergeben"]);
    $checkStmt->close();
    $conn->close();
    exit;
}

$checkStmt->close();

// Passwort hashen
$passwordHash = password_hash($password, PASSWORD_DEFAULT);

// Neuen Benutzer erstellen
$insertStmt = $conn->prepare("INSERT INTO users (username, email, password_hash) VALUES (?, ?, ?)");
$insertStmt->bind_param("sss", $username, $email, $passwordHash);

if ($insertStmt->execute()) {
    $userId = $conn->insert_id;
    $response = [
        "status" => "success",
        "user_id" => $userId,
        "message" => "Benutzer erfolgreich registriert"
    ];
} else {
    $response = [
        "status" => "error",
        "message" => "Registrierung fehlgeschlagen"
    ];
}

$insertStmt->close();
$conn->close();

echo json_encode($response);
?>