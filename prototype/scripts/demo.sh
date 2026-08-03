#!/usr/bin/env bash
set -euo pipefail

API="${API_URL:-http://localhost:8080}"
SOURCE="${SOURCE_URL:-http://localhost:5101}"
PARTNER="${PARTNER_URL:-http://localhost:5102}"

curl_check() {
  local label="$1"
  local url="$2"
  local extra_args="${3:-}"

  echo -n "  $label: "
  if response=$(curl -sS -f $extra_args "$url" 2>&1); then
    echo "$response"
    return 0
  else
    echo "FAILED"
    echo "    URL: $url"
    echo "    Hint: run 'docker compose ps' — API, postgres, and rabbitmq must be Up"
    echo "    Fix:  cd prototype && docker compose up -d"
    return 1
  fi
}

echo "== Health checks =="
curl_check "service-bus-api" "$API/health"
curl_check "partner-app" "$PARTNER/health"

echo "== Emit OrderCreated from source-app =="
curl_check "emit event" "$SOURCE/emit/OrderCreated" "-X POST"

echo "== Waiting for delivery =="
sleep 3

echo "== Partner received events =="
curl_check "received" "$PARTNER/received"

echo "== Demo complete =="
