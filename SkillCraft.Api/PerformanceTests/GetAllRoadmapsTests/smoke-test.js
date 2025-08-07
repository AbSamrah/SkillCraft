import { getAllRoadmapsTest } from '../common.js';

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'constant-vus',
    vus: 1,
    duration: '10s',
    thresholds: {
        'http_req_failed': ['rate<0.01'],   // Failures should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
    },
};

// --- The Main Test Function ---
export default getAllRoadmapsTest;