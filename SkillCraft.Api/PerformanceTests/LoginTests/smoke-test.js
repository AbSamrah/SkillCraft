import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Create a custom trend metric to track login duration.
const loginDuration = new Trend('login_duration');

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'constant-vus',
    vus: 1,
    duration: '10s',
    gracefulStop: '5s',
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
