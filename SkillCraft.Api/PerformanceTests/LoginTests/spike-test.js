import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Create a custom trend metric to track login duration.
const loginDuration = new Trend('login_duration');

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'ramping-vus',
    stages:
    [
        { duration: '1m', target: 200 }, // Start with 200 users
        { duration: '10s', target: 500 }, // Spike to 200 users
        { duration: '1m', target: 500 }, // Hold
        { duration: '10s', target: 0 }, // Ramp down to 0
    ],
    gracefulRampDown: '30s',
    thresholds:
    {
    'http_req_failed': ['rate<0.01'],   // http errors should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
        'login_duration': ['p(95)<800'],   // 95% of login operations should be below 800ms
    },
};

// --- The Main Test Function ---
export default function () {
    const url = 'http://localhost:5093/api/Auth/login';

const payload = JSON.stringify({
        email: 'ammar@gmail.com',
    password: '12345678',
    });

const params = {
        headers: {
    'Content-Type': 'application/json',
        },
    };

const res = http.post(url, payload, params);

loginDuration.add(res.timings.duration);

check(res, {
    'login successful (status 200)': (r) => r.status === 200,
        'response body contains token': (r) => r.body.length > 100,
    });

sleep(1);
}
