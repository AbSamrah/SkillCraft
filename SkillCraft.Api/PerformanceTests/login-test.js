import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Create a custom trend metric to track login duration.
const loginDuration = new Trend('login_duration');

// --- Test Scenarios ---
// We define different scenarios for each type of test.
// You can run a specific scenario from the command line.
export const options = {
    scenarios: {
        // 1. Smoke Test: A quick test with 1 user to ensure the endpoint is working.
        // k6 run --scenario smoke_test login-test.js
        smoke_test: {
            executor: 'constant-vus',
            vus: 1,
            duration: '10s',
            gracefulStop: '5s',
        },
        // 2. Load Test: Simulates normal user traffic.
        // Ramps up to 100 users over 1 minute, stays there for 5 minutes, then ramps down.
        // k6 run --scenario load_test login-test.js
        load_test: {
            executor: 'ramping-vus',
            startTime: '15s', // Start after the smoke test
            stages: [
                { duration: '1m', target: 100 }, // Ramp up to 100 users
                { duration: '5m', target: 100 }, // Stay at 100 users
                { duration: '1m', target: 0 },   // Ramp down to 0
            ],
            gracefulRampDown: '30s',
        },
        // 3. Stress Test: Pushes the system to its limits to find the breaking point.
        // Ramps up to 400 users quickly.
        // k6 run --scenario stress_test login-test.js
        stress_test: {
            executor: 'ramping-vus',
            startTime: '7m', // Start after the load test
            stages: [
                { duration: '2m', target: 400 },
                { duration: '1m', target: 400 },
                { duration: '1m', target: 0 },
            ],
            gracefulRampDown: '30s',
        },
        // 4. Spike Test: Simulates sudden, massive bursts of traffic.
        // k6 run --scenario spike_test login-test.js
        spike_test: {
            executor: 'ramping-vus',
            startTime: '11m', // Start after the stress test
            stages: [
                { duration: '10s', target: 200 }, // Spike to 200 users
                { duration: '1m', target: 200 }, // Hold
                { duration: '10s', target: 0 },   // Ramp down
            ],
            gracefulRampDown: '30s',
        },
        // 5. Soak Test: A long-running test to check for memory leaks or performance degradation.
        // NOTE: A real soak test should run for several hours (e.g., '4h').
        // k6 run --scenario soak_test login-test.js
        soak_test: {
            executor: 'constant-vus',
            startTime: '13m', // Start after the spike test
            vus: 50,
            duration: '15m', // Using 15 minutes for this example
            gracefulStop: '5m',
        },
    },
    thresholds: {
        // Define performance goals (Service Level Objectives - SLOs)
        'http_req_failed': ['rate<0.01'], // http errors should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
        'login_duration': ['p(95)<800'], // 95% of login operations should be below 800ms
    },
};

// --- The Main Test Function ---
// This is the code that each Virtual User (VU) will execute.
export default function () {
    const url = 'http://localhost:5093/api/Auth/login';

    // Define the payload with your test user's credentials
    const payload = JSON.stringify({
        email: 'ammar@gmail.com',
        password: '12345678',
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    // Send the POST request
    const res = http.post(url, payload, params);

    // Add the request duration to our custom metric
    loginDuration.add(res.timings.duration);

    // Check the response
    check(res, {
        'login successful (status 200)': (r) => r.status === 200,
        'response body contains token': (r) => r.body.length > 100, // A simple check for a JWT
    });

    // Wait for 1 second before the next iteration to simulate user think time
    sleep(1);
}
