<script setup>
    // Props
    const props = defineProps({
        user: {
            type: Object,
            required: true,
        },
    });
</script>

<template>
    <div class="user-card">
        <p class="user-name">{{ props.user.name }}</p>
        <p class="user-email">{{ props.user.email }}</p>
    </div>
</template>

<style scoped>
    .user-card {
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 8px;
        background-color: #fff;
        cursor: pointer;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        width: 250px;
    }

    .user-card:hover {
        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.15);
    }

    .user-name {
        font-weight: bold;
        font-size: 1.1rem;
        color: #333;
    }

    .user-email {
        font-size: 1rem;
        color: #555;
    }
</style>
