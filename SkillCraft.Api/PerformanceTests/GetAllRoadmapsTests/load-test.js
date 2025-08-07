import { getAllRoadmapsTest } from '../common.js';


// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'ramping-vus',
    stages: [
        { duration: '1m', target: 100 }, // Ramp up to 100 users
        { duration: '5m', target: 100 }, // Stay at 100 users
        { duration: '1m', target: 0 },   // Ramp down to 0
    ],
    gracefulRampDown: '30s',
    thresholds: {
        'http_req_failed': ['rate<0.01'],   // http errors should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
    },
};

// --- The Main Test Function ---
export default getAllRoadmapsTest;