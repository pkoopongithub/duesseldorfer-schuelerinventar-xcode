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

// Transaktion beginnen für atomare Updates
$conn->begin_transaction();

try {
    foreach ($data as $item) {
        $kompetenzID = (int)$item['kompetenzID'];
        $p1 = (float)$item['p1'];
        $p2 = (float)$item['p2'];
        $p3 = (float)$item['p3'];
        $p4 = (float)$item['p4'];
        $p5 = (float)$item['p5'];
        
        $stmt = $conn->prepare("UPDATE normSEhs SET p1 = ?, p2 = ?, p3 = ?, p4 = ?, p5 = ? WHERE kompetenzID = ?");
        $stmt->bind_param("dddddi", $p1, $p2, $p3, $p4, $p5, $kompetenzID);
        $stmt->execute();
        $stmt->close();
    }
    
    $conn->commit();
    echo json_encode(["status" => "success"]);
} catch (Exception $e) {
    $conn->rollback();
    echo json_encode(["status" => "error", "message" => $e->getMessage()]);
}

$conn->close();
?>