<script setup>
    import { RouterLink } from 'vue-router';

    // Props
    const props = defineProps({
        members: {
            type: Array,
            required: true,
            default: () => []
        },
        projectId: {
            type: Number,
            required: true,
        },
        isLeader: {
            type: Boolean,
            required: true,
        },
    });
    const userId = localStorage.getItem('userId');
</script>


<template>
    <div class="table-of-members">
        <table class="members-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Role</th>
                    <th>Task</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="member in members" :key="member.userId">
                    <td>{{ member.userName }}</td>
                    <td>{{ member.userEmail }}</td>
                    <td>{{ member.role }}</td>
                    <td>
                        <RouterLink v-if="isLeader || userId == member.userId"
                            :to="`/${userId}/projects/project-id=${projectId}/tasks/memberId=${member.userId}`"
                            class="inline-block px-4 py-2 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-offset-2">
                            View
                        </RouterLink>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>


<style scoped>
    .table-of-members {
        margin-top: 16px;
        overflow-x: auto;
    }

    .members-table {
        width: 100%;
        border-collapse: collapse;
    }

    .members-table th,
    .members-table td {
        padding: 12px 16px;
        text-align: left;
        border: 1px solid #ddd;
    }

    .members-table th {
        background-color: #f4f4f4;
        font-weight: bold;
    }

    .members-table tbody tr:nth-child(odd) {
        background-color: #f9f9f9;
    }

    .members-table tbody tr:hover {
        background-color: #f1f1f1;
    }
</style>
