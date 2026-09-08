package com.demo.controller;

import com.demo.model.Customer;
import com.demo.repository.CustomerSpringDataRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

/**
 * REST API for managing {@link Customer} resources.
 *
 * Base path: {@code /api/v2/customers}
 */
@RestController
@RequestMapping("/api/v2/customers")
public class CustomerApiV2Controller {

    private final CustomerSpringDataRepository customerRepository;

    @Autowired
    public CustomerApiV2Controller(CustomerSpringDataRepository customerRepository) {
        this.customerRepository = customerRepository;
    }

    /**
     * Retrieve all customers.
     */
    @GetMapping
    public ResponseEntity<List<Customer>> getAllCustomers() {
        List<Customer> customers = customerRepository.findAll();
        return ResponseEntity.ok(customers);
    }

    /**
     * Retrieve a customer by its ID.
     */
    @GetMapping("/{id}")
    public ResponseEntity<Customer> getCustomerById(@PathVariable Long id) {
        Optional<Customer> customerOpt = customerRepository.findById(id);
        return customerOpt.map(ResponseEntity::ok)
                .orElseGet(() -> ResponseEntity.status(HttpStatus.NOT_FOUND).build());
    }

    /**
     * Create a new customer.
     */
    @PostMapping
    public ResponseEntity<Customer> createCustomer(@RequestBody Customer customer) {
        if (customer.getId() != null && customerRepository.existsById(customer.getId())) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        Customer saved = customerRepository.save(customer);
        return ResponseEntity.status(HttpStatus.CREATED).body(saved);
    }

    /**
     * Update an existing customer.
     */
    @PutMapping("/{id}")
    public ResponseEntity<Customer> updateCustomer(@PathVariable Long id, @RequestBody Customer updatedCustomer) {
        return customerRepository.findById(id)
                .map(existing -> {
                    existing.setName(updatedCustomer.getName());
                    existing.setEmail(updatedCustomer.getEmail());
                    // Copy other mutable fields as needed.
                    Customer saved = customerRepository.save(existing);
                    return ResponseEntity.ok(saved);
                })
                .orElseGet(() -> ResponseEntity.status(HttpStatus.NOT_FOUND).build());
    }

    /**
     * Delete a customer by ID.
     */
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteCustomer(@PathVariable Long id) {
        if (!customerRepository.existsById(id)) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }
        customerRepository.deleteById(id);
        return ResponseEntity.noContent().build();
    }
}
