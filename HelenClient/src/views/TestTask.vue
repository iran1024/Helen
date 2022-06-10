<template>
    <div class="card">
        
    </div>
</template>

<script>
import { ref } from 'vue';

export default {
  setup(props, {emit}) {
    emit('routed', true, false);

  }
}
</script>