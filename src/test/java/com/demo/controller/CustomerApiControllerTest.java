package com.demo.controller;

import com.demo.model.Customer;
import com.demo.repository.CustomerJpaRepository;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.mockito.BDDMockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.util.Arrays;
import java.util.Optional;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@WebMvcTest(CustomerApiController.class)
class CustomerApiControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @MockBean
    private CustomerJpaRepository customerRepository;

    @Test
    @DisplayName("GET /api/customers - returns list of customers")
    void testGetAllCustomers() throws Exception {
        Customer c1 = new Customer(1L, "John Doe", "john@example.com");
        Customer c2 = new Customer(2L, "Jane Smith", "jane@example.com");
        BDDMockito.given(customerRepository.findAll()).willReturn(Arrays.asList(c1, c2));

        mockMvc.perform(get("/api/customers"))
                .andExpect(status().isOk())
                .andExpect(content().contentType(MediaType.APPLICATION_JSON))
                .andExpect(jsonPath("$[0].id").value(1))
                .andExpect(jsonPath("$[0].name").value("John Doe"))
                .andExpect(jsonPath("$[1].id").value(2))
                .andExpect(jsonPath("$[1].email").value("jane@example.com"));
    }

    @Test
    @DisplayName("GET /api/customers/{id} - returns customer when found")
    void testGetCustomerById_Found() throws Exception {
        Customer c = new Customer(1L, "Alice", "alice@example.com");
        BDDMockito.given(customerRepository.findById(1L)).willReturn(Optional.of(c));

        mockMvc.perform(get("/api/customers/1"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(1))
                .andExpect(jsonPath("$.name").value("Alice"))
                .andExpect(jsonPath("$.email").value("alice@example.com"));
    }

    @Test
    @DisplayName("GET /api/customers/{id} - returns 404 when not found")
    void testGetCustomerById_NotFound() throws Exception {
        BDDMockito.given(customerRepository.findById(99L)).willReturn(Optional.empty());

        mockMvc.perform(get("/api/customers/99"))
                .andExpect(status().isNotFound());
    }

    @Test
    @DisplayName("POST /api/customers - creates a new customer")
    void testCreateCustomer() throws Exception {
        Customer input = new Customer(null, "Bob", "bob@example.com");
        Customer saved = new Customer(3L, "Bob", "bob@example.com");
        BDDMockito.given(customerRepository.save(input)).willReturn(saved);

        mockMvc.perform(post("/api/customers")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(input)))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(3))
                .andExpect(jsonPath("$.name").value("Bob"))
                .andExpect(jsonPath("$.email").value("bob@example.com"));
    }
}
